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
    class VisualRecognitionManager
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
            this.timedScreenshotTaker.Start();
        }

        /* Update spawn location for the card select dialog (could have changed) */
        private void UpdateCardMatchDialogSpawnLocation()
        {
            Rectangle winRect = table.WindowRect;
            matcher.SetCardMatchDialogSpawnLocation(winRect.X + 30, winRect.Y + 30);
        }

        void timedScreenshotTaker_ScreenshotTaken(Bitmap screenshot)
        {
            UpdateCardMatchDialogSpawnLocation();

            /* This code would resize the map and recompute the data in it,
             * but we don't use this approach anymore. */
            //recognitionMap.AdjustToSize(screenshot.Size);

            /* Instead if the screenshot we took differs in size from the map at our disposal
             * we resize the window and retake the screenshot */
            if (!screenshot.Size.Equals(recognitionMap.OriginalMapSize))
            {
                Trace.WriteLine(String.Format("Screenshot size ({0}x{1}) differs from our map image ({2}x{3}), resizing window...", 
                    screenshot.Size.Width, screenshot.Size.Height, recognitionMap.OriginalMapSize.Width, recognitionMap.OriginalMapSize.Height));

                Size winSize = tableWindow.Size;

                Size difference = new Size(screenshot.Size.Width - recognitionMap.OriginalMapSize.Width,
                                        screenshot.Size.Height - recognitionMap.OriginalMapSize.Height);

                Size newSize = winSize - difference;

                tableWindow.Resize(newSize, true);
                Trace.WriteLine(" --- CurrentHeroSeat: " + table.CurrentHeroSeat);
                Trace.WriteLine(" --- resizing window try again later");
                return; // At next iteration this code should not be executed because sizes will be the same, unless the player resizes the window
            }

            if (this.processingScreenShot)
            {
                Globals.Director.WriteDebug("Processing Existing screenshot, returning");
            }
            else
            {
                processingScreenShot = true;
            }

            int heroSeat = table.CurrentHeroSeat == 0 ? 1 : table.CurrentHeroSeat;
            // If we don't know where the player is seated, we don't need to process any further
            if (table.CurrentHeroSeat == 0)
            {
                Trace.WriteLine(" --- could not find CurrentHeroSeat???");
            //    return;
            }

            /* Try to match player cards */
            List<Bitmap> playerCardImages = new List<Bitmap>();
            ArrayList playerCardsActions = colorMap.GetPlayerCardsActions(heroSeat);

            foreach(String action in playerCardsActions){
                Trace.WriteLine(" --- PlayerCardsActions: " + action);
                Rectangle actionRect = recognitionMap.GetRectangleFor(action);
                if (!actionRect.Equals(Rectangle.Empty))
                {
                    Trace.WriteLine(" --- Found Rectangle for:: " + action);
                    playerCardImages.Add(ScreenshotTaker.Slice(screenshot, actionRect));
                }
                else
                {
                    Trace.WriteLine("Warning: could not find a rectangle for action " + action);
                }
            }

            Trace.WriteLine("Matching player cards! ");
            Trace.WriteLine(" -- checking for card: " + playerCardsActions[0]);
            Card card1 = matcher.MatchCard(playerCardImages[0], playerCardsActions[0].ToString());
            Card card2 = matcher.MatchCard(playerCardImages[1], playerCardsActions[1].ToString());

            bool hasCard1 = null != card1;
            bool hasCard2 = null != card2;
            
            Trace.WriteLine("--- hasCard1: " + hasCard1 + " hasCard2: " + hasCard2);

            if (hasCard1 && hasCard2)
            {
                Trace.WriteLine("---- HAS CARDSSSSS!!! YEA!!!!");
                CardList playerCards = new CardList(2);
                playerCards.AddCard(card1);
                playerCards.AddCard(card2);
            
                //CardList playerCards = matcher.MatchCards(playerCardImages, false);
                // if (playerCards != null)
                // {
                    Trace.WriteLine("Matched player cards! " + playerCards.ToString());
                    handler.PlayerHandRecognized(playerCards);
                // }
                // else
                // {
                //     Trace.WriteLine(" --- Did not find any matching player cards ");
                // }
    
            }
            else
            {
                Trace.WriteLine(" --- Did not find any matching player cards ");
            }
            
            // Dispose
            foreach (Bitmap image in playerCardImages) if (image != null) image.Dispose();

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
                        Trace.WriteLine("Warning: could not find a rectangle for action " + action);
                    }
                }
            
                // We try to identify as many cards as possible
                CardList communityCards = matcher.MatchCards(communityCardImages, true);
                if (communityCards != null && communityCards.Count > 0)
                {
                    Trace.WriteLine("~~~ Matched board cards! " + communityCards.ToString());
                    handler.BoardRecognized(communityCards);
                }
                else
                {
                    Trace.WriteLine("~~~ Warning: could not find a commnity cards ");
                }
            
                // Dispose
                foreach (Bitmap image in communityCardImages) if (image != null) image.Dispose();
            }

            // Dispose screenshot
            if (screenshot != null) screenshot.Dispose();

            processingScreenShot = false;
        }

        public void Cleanup(){
            if (timedScreenshotTaker != null) timedScreenshotTaker.Stop();
        }


    }
}
