﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace PokerMuck
{
    //TODO: extend to have base Web Poker Client
    class AdvancedPokerTrainer : PokerClient
    {
        private String domainValue = "advancedpokertraining";
        public AdvancedPokerTrainer()
        {
        }

        public AdvancedPokerTrainer(String language)
        {
            InitializeLanguage(language);
        }

        public override double MatchTemplateThreshold()
        {
            return 0.7d;
        }

        public override double MatchHistogramThreshold()
        {
            return 0.0029d;
        }

        public override double AllowableMatchTemplateThreshold()
        {
            return 0.96d;
        }

        protected override void InitializeData()
        {
            if (CurrentLanguage == "English")
            {
                //TODO: refactor this out of "random" strings and to Vars that
                // can be used with Intellisense...
                
                // Get the domain if needed
                regex.Add("game_window_recognize_domain", @"^(https:\/\/)(?:www\.)(?<domain>.*)\.(?:com|au\.uk|co\.in)");

                
                /* These types are live games that do ***not*** have Hand History */
                regex.Add("game_window_title_to_recognize_9_max", @"^(.*)\/poker\/(?<type>game?\.)");
                regex.Add("game_window_title_to_recognize_6_max", @"^(.*)\/poker\/(?<type>sixmax?\.)");
                regex.Add("game_window_title_to_recognize_heads_up", @"^(.*)\/poker\/(?<type>headsup?\.)");
                regex.Add("game_window_title_to_recognize_sng", @"^(.*)\/poker\/(?<type>sng?\.)");                
                regex.Add("game_window_title_to_recognize_tournament", @"^(.*)\/poker\/(?<type>mtt_full?\.)");
                regex.Add("game_window_title_to_recognize_final_table", @"^(.*)\/poker\/(?<type>mtt_ft?\.)");
                //regex.Add("game_window_title_to_recognize_cash_game_description", );

                /* Session Replay games will have a hand history */
                // this is for a session replay
                regex.Add("game_window_recognize_session_replay_id",
                    @"^(https:\/\/)(?:www\.)(?<domain>.*)\.(?:com|au\.uk|co\.in)(.*?\?)(?<name>[^&=]+)=(?<value>[^&=]+)");
                
                /* APT uses PokerStars format */
                /* Recognize the Hand History game phases */
                regex.Add("hand_history_begin_preflop_phase_token", @"\*\*\* HOLE CARDS \*\*\*");
                regex.Add("hand_history_begin_flop_phase_token", @"\*\*\* FLOP \*\*\* \[(?<flopCards>[\d\w ]+)\]");
                regex.Add("hand_history_begin_turn_phase_token", @"\*\*\* TURN \*\*\* \[([\d\w ]+)\] \[(?<turnCard>[\d\w ]+)\]");
                regex.Add("hand_history_begin_river_phase_token", @"\*\*\* RIVER \*\*\* \[([\d\w ]+)\] \[(?<riverCard>[\d\w ]+)\]");
                regex.Add("hand_history_begin_showdown_phase_token", @"\*\*\* SHOW DOWN \*\*\*");
                regex.Add("hand_history_begin_summary_phase_token", @"\*\*\* SUMMARY \*\*\*");


                /* Recognize the Hand History gameID 
                 Ex. PokerStars Game #59534069543: Tournament #377151618
                 */
                regex.Add("hand_history_game_id_token", @"PokerStars (Game|Hand) #(?<handId>[\d]+): (Tournament #(?<gameId>[\d]+)|(?<gameId>.+ \(.[\d\.]+\/.[\d\.]+\)))");

                /* Recognize the table ID and max seating capacity */
                regex.Add("hand_history_table_token", @"Table '(?<tableId>.+)' (?<tableSeatingCapacity>[\d]+)-max");

                /* Recognize game (Hold'em, Omaha, No-limit, limit, etc.) 
                   Note that for PokerStars.it the only valid currency is EUR (and FPP), but this might be different 
                   on other clients. This regex works for both play money and tournaments
                 * ex. PokerStars Game #66066115721:  Hold'em No Limit (5/10) - 2011/08/16 0:15:57 CET [2011/08/15 18:15:57 ET]
                 *     PokerStars Hand #73955493608:  Hold'em No Limit (25/50) - 2012/01/16 19:16:20 CET [2012/01/16 13:16:20 ET]
                 *     PokerStars Game #69014135963: Tournament #455266512, 300+20 Hold'em No Limit - Level I (10/20) - 2011/10/15 20:20:37 EET [2011/10/15 13:20:37 ET]*/
                regex.Add("hand_history_game_token", @"(PokerStars (Game|Hand) #[\d]+: Tournament #[\d]+, .?[\d\.]+\+.?[\d\.]+ (?<gameType>[^-]+) -)|([\d]+FPP (?<gameType>[^-]+) -)|((EUR|USD) (?<gameType>[^-]+) -)|(PokerStars (Game|Hand) #[\d]+:  (?<gameType>[^(]+) \(.?[\d\.]+\/.?[\d\.]+( [\w]{3})?\))");

                /* Recognize players 
                 Ex. Seat 1: stallion089 (2105 in chips) => 1,"stallion089" 
                 * It ignores those who are marked as "out of hand"
                 */
                regex.Add("hand_history_detect_player_in_game", @"Seat (?<seatNumber>[\d]+): (?<playerName>.+) .*\(.?[\d\.]+ in chips\)$");

                /* Recognize mucked hands
                 Ex. Seat 1: stallion089 (button) (small blind) mucked [5d 5s]
                 Or: piedesa: shows [Td 6d]*/
                // TODO! What if a player has a "(" in his nickname?
                regex.Add("hand_history_detect_mucked_hand", @"((Seat [\d]+: (?<playerName>[^(]+) .*(showed|mucked) \[(?<cards>[\d\w ]+)\])|(?<playerName>.+): shows \[(?<cards>[\d\w ]+)\])");

                /* Recognize winners of a hand 
                 * Ex. cord80 collected 40 from pot */
                regex.Add("hand_history_detect_hand_winner", @"(?<playerName>.+) collected .?[\d\.]+ from pot");

                /* Recognize all-ins
                 * Ex. stallion089: raises 605 to 875 and is all-in */
                regex.Add("hand_history_detect_all_in_push", @"(?<playerName>[^:]+): .+ is all-in");

                /* Recognize the final board */
                regex.Add("hand_history_detect_final_board", @"Board \[(?<cards>[\d\w ]+)\]");

                /* Detect who is the small/big blind
                   Ex. stallion089: posts small blind 15 */
                regex.Add("hand_history_detect_small_blind", @"(?<playerName>[^:]+): posts small blind .?(?<smallBlindAmount>[\d\.]+)");
                regex.Add("hand_history_detect_big_blind", @"(?<playerName>[^:]+): posts big blind .?(?<bigBlindAmount>[\d\.]+)");

                /* Detect who the button is */
                regex.Add("hand_history_detect_button", @"#(?<seatNumber>[\d]+) is the button");

                /* Detect who our hero is (what's his nickname) */
                regex.Add("hand_history_detect_hero_name", @"Dealt to (?<heroName>.+) \[[\w\d ]+\]$");

                /* Detect calls
                 * ex. stallion089: calls 10 */
                regex.Add("hand_history_detect_player_call", @"(?<playerName>[^:]+): calls .?(?<amount>[\d\.]+)");

                /* Detect bets 
                   ex. stallion089: bets 20 */
                regex.Add("hand_history_detect_player_bet", @"(?<playerName>[^:]+): bets .?(?<amount>[\d\.]+)");

                /* Detect folds
                 * ex. preferiti90: folds */
                regex.Add("hand_history_detect_player_fold", @"(?<playerName>[^:]+): folds");

                /* Detect checks
                 * ex. DOTTORE169: checks */
                regex.Add("hand_history_detect_player_check", @"(?<playerName>[^:]+): checks");

                /* Detect raises 
                 * ex. zanzara za: raises 755 to 1155 and is all-in */
                regex.Add("hand_history_detect_player_raise", @"(?<playerName>[^:]+): raises .?([\d\.]+) to .?(?<raiseAmount>[\d\.]+)");

                /* Recognize end of round character sequence (in PokerStars.it it's
                 * a blank line */
                regex.Add("hand_history_detect_end_of_round", @"^$");

                /* Hand history file format. Example: HH20111216 T123456789 ... .txt */
                config.Add("hand_history_tournament_filename_format", "HH[0-9]+ {0}{1}");
                config.Add("hand_history_play_and_real_money_filename_format", "{0}");

                /* Game description (as shown in the hand history) */
                regex.Add("game_description_holdem", "(Hold'em No Limit|Hold'em Limit)");

            }

            /* Number of sequences required to raise the OnRoundHasTerminated event.
             * This refers to the hand_history_detect_end_of_round regex, on PokerStars.it
             * a round is over after 3 blank lines. Most clients might have only one line */
            config.Add("hand_history_end_of_round_number_of_tokens_required", 3);

        }

        /* Given a game description, returns the corresponding PokerGame */
        public override PokerGame GetPokerGameFromGameDescription(string gameDescription)
        {
            Globals.Director.WriteDebug("Found game description: " + gameDescription);

            Match match = GetRegex("game_description_holdem").Match(gameDescription);
            if (match.Success)
            {
                Globals.Director.WriteDebug("-- YES Found game description: " + gameDescription);
                return PokerGame.Holdem;
            }

            Globals.Director.WriteDebug("-- NO NOT Found game description: " + gameDescription);
            return PokerGame.Unknown; //Default
        }

        /**
         * This function matches an open window title with patterns to recognize which hand history
         * the current window refers to (if it is even a poker game window). It will return an empty
         * string if it cannot match any pattern 
         
        With ADT, we can:
        - 1, parse the title to pull out the session_replay_id and find the session test
         */
        public override String GetHandHistoryFilenameRegexPatternFromWindowTitle(String windowTitle)
        {
            // TODO REMOVE IN PRODUCTION
            if (windowTitle == "test.txt - Notepad") return "test.txt";
            if (windowTitle == "test2.txt - Notepad") return "test2.txt";
            if (windowTitle == "test3.txt - Notepad") return "test3.txt";
            if (windowTitle == "test4.txt - Notepad") return "test4.txt";
            if (windowTitle == "test5.txt - Notepad") return "test5.txt";

            
            /* 1. Are we on the correct window/domain? */
            Match aptDomainMatch = GetRegex("game_window_recognize_domain").Match(windowTitle);
            if (!aptDomainMatch.Success)
            {
                Globals.Director.WriteDebug("--- ERROR: NOT a Chromium Window pointing to a URL");
                return String.Empty;
            }
            else
            {
                string domain = aptDomainMatch.Groups["domain"].Value;
                Globals.Director.WriteDebug("--- Chromium Window pointing to a URL; domain: " + domainValue);
                if(!this.domainValue.Contains(domain))
                {
                    Globals.Director.WriteDebug("--- ERROR: Domain not correct");
                    return String.Empty;
                }
            } 
            
            /* okay APT has several different "types of games" we need to recognize:
            Live "games" (no hand history):
            1. MTT window - "game_window_title_to_recognize_tournament";
            2. 9x Cash Game window - "game_window_title_to_recognize_9_max"
            3. 6x Cash Game window - "game_window_title_to_recognize_6_max"
            4. Final Table window -  "game_window_title_to_recognize_final_table"
            5. Heads up window - "game_window_title_to_recognize_heads_up"
            6. Sit'n go table - "game_window_title_to_recognize_sng"
            
            Session Replay games, will have a hand history **
            1. game_window_recognize_session_replay_id
            Previous sessions:
            session_id=<session>
            https://www.advancedpokertraining.com/poker/game_popup.php?session_replay_id=3047262&replay_game_level=16&advisor_id=-1&html5=1&retry=1
            */ 
            /* 2. session, it is the only one with a Hand History (ATM) */
            Regex regex = GetRegex("game_window_recognize_session_replay_id");
            Match match = regex.Match(windowTitle);
            Globals.Director.WriteDebug("-- looking for a session replay id");
            if (match.Success)
            {
                // We matched a session id
                string sessionId = match.Groups["value"].Value;
                Globals.Director.WriteDebug("-- found one, session id: " + sessionId);
                
                // We matched a real money game window, need to convert the description into a filename friendly format
                return String.Format(GetConfigString("hand_history_play_and_real_money_filename_format"), sessionId);
            }
            else
            {
                Globals.Director.WriteDebug("-- DID NOT FIND A SESSION, maybe live game?");
                return String.Empty; //Could not find any valid match...
            }
        }

        public override String GetCurrentHandHistorySubdirectory()
        {
            return String.Empty; //Not necessary for PokerStars
        }

        public override bool IsPlayerSeatingPositionRelative(PokerGameType gameType)
        {
            return false;
        }

        public override PokerGameType GetPokerGameTypeFromWindowTitle(string windowTitle)
        {
            /*
             * 1. MTT window - "game_window_title_to_recognize_tournament";
            2. 9x Cash Game window - "game_window_title_to_recognize_9_max"
            3. 6x Cash Game window - "game_window_title_to_recognize_6_max"
            4. Final Table window -  "game_window_title_to_recognize_final_table"
            5. Heads up window - "game_window_title_to_recognize_heads_up"
            6. Sit'n go table - "game_window_title_to_recognize_sng"
             */
            Globals.Director.WriteDebug("-- Checking for type of game");
            Regex regex = GetRegex("game_window_title_to_recognize_tournament");
            if (regex.Match(windowTitle).Success)
            {
                Globals.Director.WriteDebug("-- found MTT");
                return PokerGameType.Tournament;
            }

            regex = GetRegex("game_window_title_to_recognize_9_max");
            if (regex.Match(windowTitle).Success)
            {
                Globals.Director.WriteDebug("-- found 9 Max");
                return PokerGameType.Ring9Max;
            }
            
            regex = GetRegex("game_window_title_to_recognize_6_max");
            if (regex.Match(windowTitle).Success)
            {
                Globals.Director.WriteDebug("-- found 6 Max");
                return PokerGameType.Ring6Max;
            }
            
            regex = GetRegex("game_window_title_to_recognize_final_table");
            if (regex.Match(windowTitle).Success)
            {
                Globals.Director.WriteDebug("-- found final table");
                return PokerGameType.FinalTable;
            }
            
            regex = GetRegex("game_window_title_to_recognize_heads_up");
            if (regex.Match(windowTitle).Success)
            {
                Globals.Director.WriteDebug("-- found headsup");
                return PokerGameType.HeadsUp;
            }
            
            regex = GetRegex("game_window_title_to_recognize_sng");
            if (regex.Match(windowTitle).Success)
            {
                Globals.Director.WriteDebug("-- found SNG");
                return PokerGameType.SNG;
            }
            
            return PokerGameType.Unknown;
        }

        public override String Name
        {
            get
            {
                return "Advanced Poker Trainer";
            }
        }

        public override string XmlName
        {
            get
            {
                return "Advanced_Poker_Trainer";
            }
        }

        public override ArrayList SupportedLanguages
        {
            get
            {
                ArrayList languages = new ArrayList();
                languages.Add("English");
                return languages;
            }
        }

        public override ArrayList SupportedGameModes
        {
            get
            {
                ArrayList supportedGameModes = new ArrayList();
                supportedGameModes.Add("No Limit Hold'em"); //TODO CHECK
                supportedGameModes.Add("Limit Hold'em");


                return supportedGameModes;
            }
        }

        protected override RegexOptions regexOptions
        {
            get
            {
                return RegexOptions.IgnoreCase | RegexOptions.Compiled;
            }
        }
    }
}
