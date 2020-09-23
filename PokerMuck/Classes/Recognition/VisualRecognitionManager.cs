using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace PokerMuck
{
    public class VisualRecognitionManager
    {
        /* How often do we pick a new screenshot and elaborate the images? */
        const int REFRESH_TIME = 2000;

        private Table table;
        private IVisualRecognitionManagerHandler handler;
        private VisualRecognitionMap recognitionMap;
        private ColorMap colorMap;
        private TimedScreenshotTaker timedScreenshotTaker;
        private VisualMatcher matcher;
        private Window tableWindow;

        private bool processingScreenShot = false;
        public VisualRecognitionManager(Table table, IVisualRecognitionManagerHandler handler)
        {
            Trace.Assert(table.Game != PokerGame.Unknown, "Cannot create a visual recognition manager without knowing the game of the table");
            Trace.Assert(table.WindowRect != Rectangle.Empty, "Cannot create a visual recognition manager without knowing the window rect");

            this.table = table;
            this.handler = handler;
            this.colorMap = ColorMap.Create(table.Game);
            this.recognitionMap = new VisualRecognitionMap(table.VisualRecognitionMapLocation, colorMap);
            this.matcher = new VisualMatcher(Globals.UserSettings.CurrentPokerClient);
            this.tableWindow = new Window(table.WindowHandle);

            this.timedScreenshotTaker = new TimedScreenshotTaker(REFRESH_TIME, tableWindow);
            this.timedScreenshotTaker.ScreenshotTaken += new TimedScreenshotTaker.ScreenshotTakenHandler(timedScreenshotTaker_ScreenshotTaken);
            
        }

        /* Update spawn location for the card select dialog (could have changed) */
        private void UpdateCardMatchDialogSpawnLocation()
        {
            Rectangle winRect = table.WindowRect;
            matcher.SetCardMatchDialogSpawnLocation(winRect.X + 30, winRect.Y + 30);
        }

        public void StartProcessingTimedScreenShots()
        {
            this.timedScreenshotTaker.Start();
        }
        
        public void StopProcessingTimedScreenShots()
        {
            this.timedScreenshotTaker.Stop();
        }
        
        void timedScreenshotTaker_ScreenshotTaken(Bitmap screenshot)
        {
            this.VisuallyProcessTableImage(screenshot);
        }

        private void ProcessCommunityCardActions(Bitmap screenshot)
        {
            /* If community cards are supported, try to match them */
            if (colorMap.SupportsCommunityCards)
            {
                List<Bitmap> communityCardImages = new List<Bitmap>();
                ArrayList communityCardsActions = colorMap.GetCommunityCardsActions();

                foreach (String action in communityCardsActions)
                {
                    Rectangle actionRect = recognitionMap.GetRectangleFor(action);
                    if (!actionRect.Equals(Rectangle.Empty))
                    {
                        communityCardImages.Add(ScreenshotTaker.Slice(screenshot, actionRect));
                    }
                    else
                    {
                        Globals.Director.WriteDebug("Warning: could not find a rectangle for action " + action);
                    }
                }

                // We try to identify as many cards as possible
                CardList communityCards = matcher.MatchCards(communityCardImages,
                    true,
                    communityCardsActions,
                    table.MatchHistogramThreshold(),
                    table.MatchTemplateThreshold(),
                    table.AllowableMatchTemplateThreshold()
                );
                if (communityCards != null && communityCards.Count > 0)
                {
                    //Globals.Director.WriteDebug("~~~ Matched board cards! " + communityCards.ToString());
                    handler.BoardRecognized(communityCards);
                }
                else
                {
                    Globals.Director.WriteDebug("~~~ Warning: could not find a commnity cards ");
                }

                // Dispose
                foreach (Bitmap image in communityCardImages)
                    if (image != null)
                        image.Dispose();
            }
        }

        private void ProcessPlayerCardActions(Bitmap screenshot, int seat, bool isHero)
        {
            /* Try to match player cards */
            List<Bitmap> playerCardImages = new List<Bitmap>();
            ArrayList playerCardsActions = colorMap.GetPlayerCardsActions(seat);

            foreach (String action in playerCardsActions)
            {
                Globals.Director.WriteDebug(" --- PlayerCardsActions: " + action);
                Rectangle actionRect = recognitionMap.GetRectangleFor(action);
                if (!actionRect.Equals(Rectangle.Empty))
                {
                    //Globals.Director.WriteDebug(" --- Found Rectangle for:: " + action);
                    playerCardImages.Add(ScreenshotTaker.Slice(screenshot, actionRect));
                }
                else
                {
                    Globals.Director.WriteDebug("Warning: could not find a rectangle for action " + action);
                }
            }

            //Globals.Director.WriteDebug("Matching player cards! ");

            //playerCardsActions
            CardList playerCards = matcher.MatchCards(playerCardImages,
                false,
                playerCardsActions,
                table.MatchHistogramThreshold(),
                table.MatchTemplateThreshold(),
                table.AllowableMatchTemplateThreshold());
            if (playerCards != null && isHero)
            {
                Globals.Director.WriteDebug("Matched player cards! " + playerCards.ToString());
                handler.PlayerHandRecognized(playerCards);
            }
            else if (playerCards != null && !isHero)
            {
                Hand hand = new Hand();
                Globals.Director.WriteDebug(" -- NOT hero cards. Seat " + seat + " Cards: " + playerCards.ToString());
            }
            else
            {
                Globals.Director.WriteDebug(" --- SEAT: " + seat + " Did not find any matching player cards ");
            }

            // Dispose
            foreach (Bitmap image in playerCardImages)
                if (image != null)
                    image.Dispose();
        }

        public bool VisuallyProcessTableImage(Bitmap screenshot)
        {
            UpdateCardMatchDialogSpawnLocation();

            /* This code would resize the map and recompute the data in it,
             * but we don't use this approach anymore. */
            //recognitionMap.AdjustToSize(screenshot.Size);

            /* Instead if the screenshot we took differs in size from the map at our disposal
             * we resize the window and retake the screenshot */
            if (!screenshot.Size.Equals(recognitionMap.OriginalMapSize))
            {
                // Globals.Director.WriteDebug(String.Format("Screenshot size ({0}x{1}) differs from our map image ({2}x{3}), resizing window...", 
                //     screenshot.Size.Width, screenshot.Size.Height, recognitionMap.OriginalMapSize.Width, recognitionMap.OriginalMapSize.Height));

                Size winSize = tableWindow.Size;

                Size difference = new Size(screenshot.Size.Width - recognitionMap.OriginalMapSize.Width,
                                        screenshot.Size.Height - recognitionMap.OriginalMapSize.Height);

                Size newSize = winSize - difference;

                tableWindow.Resize(newSize, true);
                // Globals.Director.WriteDebug(" --- CurrentHeroSeat: " + table.CurrentHeroSeat);
                // Globals.Director.WriteDebug(" --- resizing window try again later");
                return false; // At next iteration this code should not be executed because sizes will be the same, unless the player resizes the window
            }

            if (this.processingScreenShot)
            {
                //Globals.Director.WriteDebug("Processing Existing screenshot, returning");
                // Dispose screenshot
                if (screenshot != null) 
                    screenshot.Dispose();
                return false;
            }
            else
            {
                processingScreenShot = true;
            }

            int heroSeat = table.CurrentHeroSeat == 0 ? 1 : table.CurrentHeroSeat;
            // If we don't know where the player is seated, we don't need to process any further
            if (table.CurrentHeroSeat == 0)
            {
                Globals.Director.WriteDebug(" ERROR: --- could not find CurrentHeroSeat???");
                return false;
            }
            
            foreach (Player player in table.PlayerList)
            {
                int seat = player.SeatNumber;
                ProcessPlayerCardActions(screenshot, seat, seat == heroSeat);
            }

            ProcessCommunityCardActions(screenshot);
             
            // Dispose screenshot
            if (screenshot != null) screenshot.Dispose();

            processingScreenShot = false;

            return true;
        }

        // DO NOT NEED TO USE, here for completeness
        public void ProcessHeroHands(Bitmap screenshot, int heroSeat)
        {
            /* Try to match player cards */
            List<Bitmap> playerCardImages = new List<Bitmap>();
            ArrayList playerCardsActions = colorMap.GetPlayerCardsActions(heroSeat);
            
            foreach(String action in playerCardsActions){
                Globals.Director.WriteDebug(" --- PlayerCardsActions: " + action);
                Rectangle actionRect = recognitionMap.GetRectangleFor(action);
                if (!actionRect.Equals(Rectangle.Empty))
                {
                    Globals.Director.WriteDebug(" --- Found Rectangle for:: " + action);
                    playerCardImages.Add(ScreenshotTaker.Slice(screenshot, actionRect));
                }
                else
                {
                    Globals.Director.WriteDebug("Warning: could not find a rectangle for action " + action);
                }
            }
            
            Globals.Director.WriteDebug("Matching player cards! ");
            
            //playerCardsActions
            CardList playerCards = matcher.MatchCards(playerCardImages,
                false, 
                playerCardsActions, 
                table.MatchHistogramThreshold(), 
                table.MatchTemplateThreshold(),
                table.AllowableMatchTemplateThreshold());
            if (playerCards != null)
            {
                Globals.Director.WriteDebug("Matched player cards! " + playerCards.ToString());
                handler.PlayerHandRecognized(playerCards);
            }
            else
            {
                Globals.Director.WriteDebug(" --- Did not find any matching player cards ");
            }
            
            // Dispose
            foreach (Bitmap image in playerCardImages) 
                if (image != null) image.Dispose();

        }
        
        public void Cleanup(){
            if (timedScreenshotTaker != null) timedScreenshotTaker.Stop();
        }


    }
}
