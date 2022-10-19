using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
namespace JeuDeCombat
{
    struct RatioGetter
    {
        public float percentAi1;
        public float percentAi2;
        public float percentDraw;
        public void SetValue(float _percentAi1, float _percentAi2, float _percentDraw)
        {
            percentAi1 = _percentAi1;
            percentAi2 = _percentAi2;
            percentDraw = _percentDraw;
        }
    }

    class Program
    {
        static List<string> availableClass = new List<string> { "D", "H", "T", "V" };
        static bool difficulty = false;
        static void Main(string[] args)
        {
            Init();
        }

        static void Init()
        {
            int choice = -1;
            PrintWelcome();
            do
                PrintChooseGameMode();
            while (!int.TryParse(Console.ReadLine(), out choice) || choice <= 0 || choice > 3);

            switch (choice)
            {
                case 1:
                    //Lauch game
                    Console.Clear();
                    SetUpPlayerVsAi(availableClass);
                    break;
                case 2:
                    //Trigger Simulation
                    Console.Clear();
                    Simulation(availableClass);                    
                    break;
                case 3:
                    //quit
                    break;

            }
        }

        #region Coding Rooms function
        static Tuple<int, int> ResolutionAction(int actionJoueur, int actionAi, string roleJoueur, string roleIA, bool narative = false)
        {
            int playerValueModifier = 0;
            int aiValueModifier = 0;

            bool playerSkillActivated = actionJoueur == 3;
            bool aiSkillActivated = actionAi == 3;

            bool playerDefending = actionJoueur == 2;
            bool aiDefending = actionAi == 2;

            //Tour du joueur

            Console.ForegroundColor = ConsoleColor.Green;

            switch (actionJoueur)
            {
                case 1:
                    if (narative)
                        Console.WriteLine("Le " + GetFullName(roleJoueur) + " attaque!");
                    aiValueModifier -= DommageParRole(roleJoueur);
                    break;
                case 2:
                    if (narative)
                        Console.WriteLine("Le " + GetFullName(roleJoueur) + " se défend!");
                    break;
                case 3:
                    playerSkillActivated = true;
                    switch (roleJoueur)
                    {
                        case "D":
                            if (narative)
                                Console.WriteLine("Le " + GetFullName(roleJoueur) + " active sa compétence ! Il renverra tous les dégâts qu'il aura subi ce tour.");
                            break;
                        case "H":
                            if (narative)
                                Console.WriteLine("Le " + GetFullName(roleJoueur) + " active sa compétence ! Il se restaure 2 points de vie !");
                            playerValueModifier += 2;
                            break;
                        case "T":
                            if (narative)
                                Console.WriteLine("Le " + GetFullName(roleJoueur) + " active sa compétence ! Il sacrifie un point de vie pour augmenter sa force de 1 et attaque!");
                            playerValueModifier -= 1;
                            aiValueModifier -= DommageParRole(roleJoueur) + 1;
                            break;
                        case "V":
                            if (narative)
                                Console.WriteLine("Le " + GetFullName(roleJoueur) + " active sa compétence ! Il mord le cou de l'adversaire pour tenter de gagner un point de vie!");
                            aiValueModifier -= 1;

                            Random rdm = new Random();
                            var dice = rdm.Next(1, 101);

                            if (dice > 50)
                            {
                                //Rater
                                if (narative)
                                    Console.WriteLine("Le " + GetFullName(roleJoueur) + " n'a pas pu se soigner");
                            }
                            else
                            {
                                //Gagner
                                if (narative)
                                    Console.WriteLine("Le " + GetFullName(roleJoueur) + " a réussi son coup ! Il se soigne 1 point de vie !");
                                playerValueModifier += 1;
                            }
                            break;
                    }
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Red;

            switch (actionAi)
            {
                case 1:
                    if (narative)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Le " + GetFullName(roleIA) + " attaque!");
                        Console.ResetColor();
                    }
                    playerValueModifier -= DommageParRole(roleIA);
                    break;

                case 2:
                    if (narative)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Le " + GetFullName(roleIA) + " se défend!");
                        Console.ResetColor();
                    }
                    break;

                case 3:
                    aiSkillActivated = true;
                    switch (roleIA)
                    {
                        case "D":
                            if (narative)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Le " + GetFullName(roleIA) + " active sa compétence ! Il renverra tous les dégâts qu'il aura subi ce tour");
                                Console.ResetColor();
                            }
                            break;
                        case "H":
                            if (narative)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Le " + GetFullName(roleIA) + " active sa compétence ! Il se restaure 2 points de vie !");
                                Console.ResetColor();
                            }
                            aiValueModifier += 2;
                            break;
                        case "T":
                            if (narative)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Le " + GetFullName(roleIA) + " active sa compétence ! Il sacrifie un point de vie pour augmenter sa force de 1 et attaque!");
                                Console.ResetColor();
                            }
                            aiValueModifier -= 1;
                            playerValueModifier -= DommageParRole(roleIA) + 1;
                            break;
                        case "V":
                            if (narative)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Le " + GetFullName(roleIA) + " active sa compétence ! Il mord le cou de l'adversaire et regagne un point de vie !");
                                Console.ResetColor();
                            }
                            playerValueModifier += 1;

                            Random rdm = new Random();
                            var dice = rdm.Next(1, 101);

                            if (dice > 50)
                            {
                                //Rater
                                if (narative)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Le " + GetFullName(roleIA) + " n'a pas pu se soigner");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                //Gagner
                                if (narative)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Le " + GetFullName(roleIA) + " a réussi son coup ! Il se soigne 1 point de vie !");
                                    Console.ResetColor();
                                }
                                aiValueModifier += 1;
                            }
                            //playerValueModifier -= 1;
                            break;
                    }
                    break;
            }

            Console.ResetColor();

            if (playerDefending && playerValueModifier + 1 <= 0)
                playerValueModifier++;

            if (aiDefending && aiValueModifier + 1 <= 0)
                aiValueModifier++;

            if (playerSkillActivated && roleJoueur == "D")
            {
                var damageReceive = Math.Abs(playerValueModifier);
                aiValueModifier -= damageReceive;
            }

            if (aiSkillActivated && roleIA == "D")
            {
                var damageReceive = Math.Abs(aiValueModifier);
                playerValueModifier -= damageReceive;
            }
            return new Tuple<int, int>(playerValueModifier, aiValueModifier);
        }
        static int DommageParRole(string role)
        {
            var charactersDM = new Dictionary<string, int>() { { "H", 1 }, { "T", 1 }, { "D", 2 }, { "V", 2 } };
            return charactersDM[role];
        }

        #endregion

        #region Player vs AI

        static void SetUpPlayerVsAi(List<string> availableClass)
        {
            PrintWelcome();

            var choice = -1;
            Random rdm = new Random();


            var dif = 0;

            do
                ChooseDifficulty();
            while (!int.TryParse(Console.ReadLine(), out dif) || dif < 1 || dif > 2);

            difficulty = dif == 2 ? true : false;
            Console.Clear();
            PrintWelcome();
            do
                PrintChooseCharacter();
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > availableClass.Count + 1);

            if (choice == availableClass.Count + 1)
            {
                Console.Clear();
                Main(null);
                return;
            }

            var player = availableClass[choice - 1];

            Console.Clear();

            PrintWelcome();

            Console.WriteLine();
            Console.Write("Vous avez choisi d'incarner un ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(GetFullName(player).ToUpper() + '\n');
            Console.ResetColor();

            choice = rdm.Next(0, availableClass.Count);
            var ai = availableClass[choice];
            Console.Write("Quand à votre adversaire, il choisit d'incarner un ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(GetFullName(ai).ToUpper() + '\n');
            Console.ResetColor();

            var sleepTime = 5000;
            Console.WriteLine("Début du combat dans " + sleepTime / 1000 + " seconde(s)!");
            Thread.Sleep(sleepTime);
            Battle(player, ai);

        }

        static void Battle(string playerRole, string aiRole)
        {
            Console.Clear();
            PrintWelcome();
            Console.WriteLine("\nQue le combat commence !");

            var playerPv = GetHpByClass(playerRole);
            var aiPv = GetHpByClass(aiRole);
            //PrintPlayerStatus(playerRole, aiRole, playerPv, aiPv);
            int round = 0;
            while (playerPv > 0 && aiPv > 0)
            {
                round++;

                PrintRountStat(round);

                PrintPlayerStatus(playerRole, aiRole, playerPv, aiPv);

                //Boucle
                var playerChoice = 0;
                var aiChoice = 0;
                Random rdm = new Random();
                do
                    PrintChooseAction();
                while (!int.TryParse(Console.ReadLine(), out playerChoice) || playerChoice < 1 || playerChoice > 4);

                if (playerChoice == 4)
                {
                    Console.Clear();
                    Main(null);
                    return;
                    //Abandonner
                    //return;
                }


                if (!difficulty)
                    aiChoice = rdm.Next(1, 4);
                else
                {
                    switch (playerChoice)
                    {
                        case 1:
                            aiChoice = 2;
                            break;
                        case 2:
                            aiChoice = 3;
                            break;
                        case 3:
                            aiChoice = 1;
                            break;
                    }

                    if (aiChoice - DommageParRole(playerRole) <= 0)
                    {
                        aiChoice = 2;
                    }
                    if (playerPv <= DommageParRole(aiRole))
                    {
                        aiChoice = 1;
                    }

                }

                var trade = ResolutionAction(playerChoice, aiChoice, playerRole, aiRole, true);

                if (aiPv + trade.Item2 > GetHpByClass(aiRole))
                    aiPv = GetHpByClass(aiRole);
                else
                    aiPv += trade.Item2;

                if (playerPv + trade.Item1 > GetHpByClass(playerRole))
                    playerPv = GetHpByClass(playerRole);
                else
                    playerPv += trade.Item1;

                Console.WriteLine();

                PrintRoundResults(trade, playerRole, aiRole);

                Thread.Sleep(2000);

                Console.WriteLine();


            }

            if (playerPv <= 0 && aiPv <= 0)
            {
                Console.WriteLine("Égalité ! Les deux joueurs sont morts !");
                return;
            }

            if (playerPv <= 0)
                Console.WriteLine("Perdu ! L'IA vous a vaicu !");

            if (aiPv <= 0)
                Console.WriteLine("Félicitation ! Vous avez vaincu !");

            int choice = -1;

            do
                PrintTryAgain();
            while (!int.TryParse(Console.ReadLine(), out choice) || choice <= 0 || choice > 2);

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    SetUpPlayerVsAi(availableClass);
                    break;
                case 2:
                    Console.Clear();
                    Main(null);
                    break;
            }
        }

        #endregion
        
    }

}