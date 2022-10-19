using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
        static List<string> availableClass = new List<string> { "D", "H", "T" };

        static void Main(string[] args)
        {
            //var availableClass = new List<string> { "D","H","T"};
            int choice = -1;
            PrintWelcome();

            do
                PrintModeChoice();
            while (!int.TryParse(Console.ReadLine(), out choice) || choice <= 0 || choice > 3);

            switch (choice)
            {
                case 1:
                    //Lauch game
                    Console.Clear();
                    PlayerVsAi(availableClass);
                    break;
                case 2:
                    Console.Clear();
                    Simulation(availableClass);
                    //Trigger Simulation
                    break;
                case 3:
                    //quit
                    break;

            }

        }



        #region SIMULATION
        static void Simulation(List<string> availableClass)
        {
            int choice = -1;

            do
                PrintAiMode();
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5);

            if (choice == 5)
            {
                Console.Clear();
                Main(null);
                return;
            }

            int nbSimulation = -1;

            do
                PrintChooseNbOfSimulation();
            while (!int.TryParse(Console.ReadLine(), out nbSimulation) || nbSimulation < 0);

            int choice2 = -1;

            do
                PrintChooseDebug();
            while (!int.TryParse(Console.ReadLine(), out choice2) || choice2 < 1 || choice2 > 2);

            var showDebug = choice2 == 1 ? true : false;

            switch (choice)
            {
                case 1:
                    AiVsAi(availableClass[0], availableClass[1], nbSimulation, showDebug);
                    break;
                case 2:
                    AiVsAi(availableClass[0], availableClass[2], nbSimulation, showDebug);
                    break;
                case 3:
                    AiVsAi(availableClass[1], availableClass[2], nbSimulation, showDebug);
                    break;
                case 4:
                    TestAllAi(availableClass, nbSimulation);
                    break;

            }

            do
            {
                PrintTryMoreTest();
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice <= 0 || choice > 2);


            Console.Clear();
            if (choice == 1)
            {
                Simulation(availableClass);
            }
            else
                Main(null);
        }

        static void AiVsAi(string ai1, string ai2, int nbSimulation, bool showDebug)
        {
            var ratio = BattleAi(ai1, ai2, nbSimulation, showDebug);
            Console.WriteLine("Draw ratio " + ratio.percentDraw + "%");
            Console.WriteLine(ai1 + " victory ratio " + ratio.percentAi1 + "%");
            Console.WriteLine(ai2 + " victory ratio " + ratio.percentAi2 + "%");
        }

        static void TestAllAi(List<string> availableClass, int nbSimulation)
        {

            Console.WriteLine(nbSimulation + " simulations");

            //Damager VS Healer
            //Console.WriteLine("DAMAGER vs HEALER");
            var ai1 = availableClass[0];
            var ai2 = availableClass[1];
            var ratioDamagerHealer = BattleAi(ai1, ai2, nbSimulation);

            //Damager vs TANK
            //Console.WriteLine("DAMAGER vs TANK");
            ai1 = availableClass[0];
            ai2 = availableClass[2];
            var ratioDamagerTank = BattleAi(ai1, ai2, nbSimulation);

            //Healer vs TANK
            //Console.WriteLine("DAMAGER vs TANK");
            ai1 = availableClass[1];
            ai2 = availableClass[2];
            var ratioHealerTank = BattleAi(ai1, ai2, nbSimulation);


            PrintTable(ratioDamagerHealer, ratioDamagerTank, ratioHealerTank);
        }
        static RatioGetter BattleAi(string ai1, string ai2, int nbSimulation, bool debug = false)
        {

            int round = 0;
            float ai1VictoryCount = 0f;
            float ai2VictoryCount = 0f;
            float drawCount = 0f;
            Random rdm = new Random();

            for (int i = 0; i < nbSimulation; i++)
            {
                //ResetPV 
                var ai1Pv = GetHpByClass(ai1);
                var ai2Pv = GetHpByClass(ai2);

                while (ai1Pv > 0 && ai2Pv > 0)
                {
                    round++;
                    var ai1Choice = rdm.Next(1, 4);
                    var ai2Choice = rdm.Next(1, 4);

                    if (debug)
                    {
                        Console.WriteLine(ai1 + " choose " + ai1Choice);
                        Console.WriteLine(ai2 + " choose " + ai2Choice);
                    }

                    var tradeResult = ResolutionAction(ai1Choice, ai2Choice, ai1, ai2);

                    //Gestion des pv max
                    if (ai2Pv + tradeResult.Item2 > GetHpByClass(ai2))
                        ai2Pv = GetHpByClass(ai2);
                    else
                        ai2Pv += tradeResult.Item2;

                    if (ai1Pv + tradeResult.Item1 > GetHpByClass(ai1))
                        ai1Pv = GetHpByClass(ai1);
                    else
                        ai1Pv += tradeResult.Item1;


                    if (debug)
                    {
                        Console.WriteLine(ai1Pv + " pv AI1");
                        Console.WriteLine(ai2Pv + " pv AI2");
                        Console.WriteLine("=========================");
                    }
                }


                if (!(ai1Pv <= 0 && ai2Pv <= 0))
                {
                    if (ai1Pv <= 0)
                    {
                        ai2VictoryCount++;
                        if (debug)
                        {
                            Console.WriteLine("\n" + ai2 + " WON !");
                            Console.WriteLine("________________________\n");
                        }
                    }

                    if (ai2Pv <= 0)
                    {
                        ai1VictoryCount++;
                        if (debug)
                        {
                            Console.WriteLine("\n" + ai1 + " WON !");
                            Console.WriteLine("________________________\n");
                        }
                    }
                }
                else
                {
                    drawCount++;
                    if (debug)
                    {
                        Console.WriteLine(" DRAW !");
                        Console.WriteLine("________________________\n");

                    }
                }

            }

            //Console.WriteLine("DRAW : " + drawCount);
            //Console.WriteLine( ai1+ " victory : " + ai1VictoryCount);
            //Console.WriteLine(ai2 + " victory : " + ai2VictoryCount);

            //Console.WriteLine("Draw ratio : " + (drawCount / nbSimulation) * 100 + "%");
            //Console.WriteLine(ai1 + " victory percentage : " + (ai1VictoryCount / nbSimulation) * 100 + "%");
            //Console.WriteLine(ai2 + " victory percentage : " + (ai2VictoryCount / nbSimulation) * 100 + "%");
            //Console.WriteLine("==================================");

            RatioGetter ratioGetter = new RatioGetter();
            ratioGetter.SetValue(ai1VictoryCount / nbSimulation * 100,
                ai2VictoryCount / nbSimulation * 100,
                drawCount / nbSimulation * 100);
            return ratioGetter;
        }

        #endregion

        #region Player vs AI

        static void PlayerVsAi(List<string> availableClass)
        {
            PrintWelcome();

            var choice = -1;
            Random rdm = new Random();
            do
                PrintChooseCharacter();
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > availableClass.Count);
            var player = availableClass[choice - 1];
            Console.WriteLine("Vous avez choisi d'incarner un " + GetFullName(player));
            Console.WriteLine("Votre adversaire choisit son champion...");
            choice = rdm.Next(0, availableClass.Count);

            var ai = availableClass[choice];
            Console.WriteLine("Votre adversaire a choisi de prendre le " + GetFullName(ai));
            Thread.Sleep(4000);


            Battle(player, ai);

        }

        static void Battle(string playerRole, string aiRole)
        {
            Console.Clear();
            PrintWelcome();
            Console.WriteLine("Que le combat commence !\n");

            var playerPv = GetHpByClass(playerRole);
            var aiPv = GetHpByClass(aiRole);

            while (playerPv > 0 && aiPv > 0)
            {

                //Boucle
                var playerChoice = 0;
                var aiChoice = 0;
                Random rdm = new Random();
                do
                    PrintChooseAction();
                while (!int.TryParse(Console.ReadLine(), out playerChoice) || playerChoice < 1 || playerChoice > 3);
                aiChoice = rdm.Next(1, 4);

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

                Console.WriteLine();
                PrintPlayerStatus(playerRole, aiRole, playerPv, aiPv);
            }

            if (playerPv == 0 && aiPv == 0)
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
                    PlayerVsAi(availableClass);
                    break;
                case 2:
                    Console.Clear();
                    Main(null);
                    break;
            }
        }

        #endregion

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
                                Console.WriteLine("Le " + GetFullName(roleJoueur) + " active sa compétence ! Il renverra tous les dégâts qu'il aura subi ce tour");
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
                    }
                    break;
            }

            switch (actionAi)
            {
                case 1:
                    if (narative)
                        Console.WriteLine("Le " + GetFullName(roleIA) + " attaque!");
                    playerValueModifier -= DommageParRole(roleIA);
                    break;

                case 2:
                    if (narative)
                        Console.WriteLine("Le " + GetFullName(roleIA) + " se défend!");
                    break;

                case 3:

                    aiSkillActivated = true;
                    switch (roleIA)
                    {
                        case "D":
                            if (narative)
                                Console.WriteLine("Le " + GetFullName(roleIA) + " active sa compétence ! Il renverra tous les dégâts qu'il aura subi ce tour");
                            break;
                        case "H":
                            if (narative)
                                Console.WriteLine("Le " + GetFullName(roleIA) + " active sa compétence ! Il se restaure 2 points de vie !");
                            aiValueModifier += 2;
                            break;
                        case "T":
                            if (narative)
                                Console.WriteLine("Le " + GetFullName(roleIA) + " active sa compétence ! Il sacrifie un point de vie pour augmenter sa force de 1 et attaque!");
                            aiValueModifier -= 1;
                            playerValueModifier -= DommageParRole(roleIA) + 1;
                            break;
                    }
                    break;
            }


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
            var charactersDM = new Dictionary<string, int>() { { "H", 1 }, { "T", 1 }, { "D", 2 } };
            return charactersDM[role];
        }

        #endregion

        #region Helper function

        static string GetFullName(string role)
        {

            switch (role)
            {
                case "T":
                    return "Tank";
                case "H":
                    return "Healer";
                case "D":
                    return "Damager";

            }
            return string.Empty;
        }

        static int GetHpByClass(string role)
        {
            switch (role)
            {
                case "D":
                    return 3;
                case "H":
                    return 4;
                case "T":
                    return 5;
            }
            return 0;
        }

        static string FixValuePrint(float value, int fullPlace)
        {
            var nbCharacter = value.ToString().Length + 1;
            var freePlaces = fullPlace - nbCharacter;
            //var placePerSide = freePlaces / 2;

            string constructString = string.Empty;

            for (int i = 0; i <= freePlaces; i++)
            {
                if (freePlaces / 2 == i)
                {
                    constructString += value.ToString() + "%";
                    continue;
                }
                constructString += " ";
            }


            return constructString;
        }

        #endregion

        #region Print things
        static void PrintRoundResults(Tuple<int, int> trade, string playerRole, string aiRole)
        {
            if (trade.Item1 == 0 && trade.Item2 == 0)
            {
                Console.WriteLine("Personne a subi de dégat");
                return;
            }

            if (trade.Item1 < 0)
                Console.WriteLine(GetFullName(playerRole) + " a reçus " + Math.Abs(trade.Item1) + " point(s) de dégât");

            if (trade.Item1 > 0)
                Console.WriteLine(GetFullName(playerRole) + " a regagner " + Math.Abs(trade.Item1) + " point(s) de vie");

            if (trade.Item1 == 0)
                Console.WriteLine(GetFullName(playerRole) + " n'a pris aucun dégât");

            //===================

            if (trade.Item2 < 0)
                Console.WriteLine(GetFullName(aiRole) + " a reçus " + Math.Abs(trade.Item2) + " point(s) de dégât");

            if (trade.Item2 > 0)
                Console.WriteLine(GetFullName(aiRole) + " a regagner " + Math.Abs(trade.Item2) + " point(s) de vie");

            if (trade.Item2 == 0)
                Console.WriteLine(GetFullName(aiRole) + " n'a pris aucun dégât");

        }

        static void PrintPlayerStatus(string playerRole, string aiRole, int playerPv, int aiPv)
        {
            Console.WriteLine("(" + playerPv + "pvs)" + GetFullName(playerRole) + "     (" + aiPv + "pvs)" + GetFullName(aiRole));
        }

        static void PrintWelcome()
        {
            Console.WriteLine("             ^     \\    /      ^       ");
            Console.WriteLine("            / \\    )\\__/(     / \\       ");
            Console.WriteLine("           /   \\  (_\\  /_)   /   \\      ");
            Console.WriteLine("      ____/_____\\__\\@  @/___/_____\\____");
            Console.WriteLine("     |             |\\../|              |");
            Console.WriteLine("     |              \\VV/               |");
            Console.WriteLine("     |      BIENVENUE DANS L'ARÈNE     |");
            Console.WriteLine("     |_________________________________|");
            Console.WriteLine("      |    /\\ /      \\        \\ /\\    | ");
            Console.WriteLine("      |  /   V        ))       V  \\   | ");
            Console.WriteLine("      |/     `       //        '    \\ | ");
            Console.WriteLine("      `              V                '");
            Console.WriteLine("  ============================================");


        }

        static void PrintTryAgain()
        {
            Console.WriteLine("Voulez vous recommencer ?");
            Console.WriteLine("1 - Oui");
            Console.WriteLine("2 - Non");
        }

        static void PrintChooseCharacter()
        {
            Console.WriteLine("Veuillez choisir votre champion !");
            Console.WriteLine("1 - Damager" +
                            "\n    PV : " + GetHpByClass("D") +
                            "\n    Force : " + DommageParRole("D") +
                            "\n    Action spéciale : Inflige en retour les dégâts qui lui sont infligés durant ce tour." +
                            "\n    Les dégâts sont quand même subis.\n");
            Console.WriteLine("2 - Healer" +
                            "\n    PV : " + GetHpByClass("H") +
                            "\n    Force : " + DommageParRole("H") +
                            "\n    Action spéciale : Récupère 2 points de vie.\n");
            Console.WriteLine("3 - Tank" +
                            "\n    PV : " + GetHpByClass("T") +
                            "\n    Force : " + DommageParRole("T") +
                            "\n    Action spéciale : Sacrifie un de ses points de vie pour augmenter sa force d’attaque de 1 et ce uniquement durant le tour en cours.");
        }

        static void PrintChooseAction()
        {
            Console.WriteLine("Veuillez choisir une action !");
            Console.WriteLine("1 - Attaquer");
            Console.WriteLine("2 - Défendre");
            Console.WriteLine("3 - Action spéciale");
        }

        static void PrintChooseNbOfSimulation()
        {
            Console.WriteLine("Combien de simulation voulez vous faire ?");
        }

        static void PrintChooseDebug()
        {
            Console.WriteLine("Voulez vous afficher tout les combats ? ");
            Console.WriteLine("1 - Oui");
            Console.WriteLine("2 - Non");

        }

        static void PrintTable(RatioGetter ratioDamagerHealer, RatioGetter ratioDamagerTank, RatioGetter ratioHealerTank)
        {
            int allPlace = 16;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("|                |                |                |                |");
            Console.WriteLine("|      X         |    DAMAGER     |     HEALER     |    TANK        |");
            Console.WriteLine("|                |                |                |                |");
            Console.WriteLine("=====================================================================");
            Console.WriteLine("|                |                |                |                |");
            Console.WriteLine("|   DAMAGER      |       X        |" + FixValuePrint(ratioDamagerHealer.percentAi1, allPlace) + "|" + FixValuePrint(ratioDamagerTank.percentAi1, allPlace) + "|");
            Console.WriteLine("|                |                |                |                |");
            Console.WriteLine("=====================================================================");
            Console.WriteLine("|                |                |                |                |");
            Console.WriteLine("|   HEALER       |" + FixValuePrint(ratioDamagerHealer.percentAi2, allPlace) + "|       X        |" + FixValuePrint(ratioHealerTank.percentAi1, allPlace) + "|");
            Console.WriteLine("|                |                |                |                |");
            Console.WriteLine("=====================================================================");
            Console.WriteLine("|                |                |                |                |");
            Console.WriteLine("|   TANK         |" + FixValuePrint(ratioDamagerTank.percentAi2, allPlace) + "|" + FixValuePrint(ratioHealerTank.percentAi2, allPlace) + "|       X        |");
            Console.WriteLine("|                |                |                |                |");
            Console.WriteLine("=====================================================================");

        }


        static void PrintModeChoice()
        {
            Console.WriteLine("Que voulez-vous faire ?");
            Console.WriteLine("1 - Jouer contre l'ordinateur ");
            Console.WriteLine("2 - Simulation IA vs IA ");
            Console.WriteLine("3 - Quitter ");

        }

        static void PrintAiMode()
        {
            Console.WriteLine("Que voulez-vous tester ?");
            Console.WriteLine("1 - Damager vs Healer");
            Console.WriteLine("2 - Damager vs Tank");
            Console.WriteLine("3 - Tank vs Healer");
            Console.WriteLine("4 - Tout tester");
            Console.WriteLine("5 - Retour");

        }

        static void PrintTryMoreTest()
        {
            Console.WriteLine("\nVoulez tester autre chose ?");
            Console.WriteLine("1 - Oui");
            Console.WriteLine("2 - Non");
        }

        #endregion

    }

}
