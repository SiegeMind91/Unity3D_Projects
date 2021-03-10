using UnityEngine;
using system = System; //Here I'm using an alias for the System namespace because there are two "Random" function definitions and this allows me to specify which I want to use

public class Hacker : MonoBehaviour
{
    //Function variables
    int level;
    string password;
    string scrambledPassword;
    string[] level1Passwords = {"donuts", "jail", "holster", "uniform", "handcuffs", "bond"}; //Words to do with a police station
    string[] level2Passwords = {"sobriety", "alphabet", "seatbelt", "pavement", "arrested", "citation"}; //Words to do with State Highway Patrol
    string[] level3Passwords = {"investigation", "criminal", "cyberpunk", "fidelity", "anonymity", "firearm"}; //Words to do with the FBI
    enum Screen { Instructions, MainMenu, Password, Win };
    Screen currentScreen = Screen.Instructions;

    // Start is called before the first frame update
    void Start()
    {
        ShowFirstScreen();
    }

    void OnUserInput(string input)
    {
        if (currentScreen == Screen.Instructions)
        {
            ProcessInstructionsOptions();
        }
        else if (currentScreen == Screen.MainMenu)
        {
            ProcessMainMenuOptions(input);
        }
        else if (currentScreen == Screen.Password)
        {
            ProcessPasswordGuess(input);
        }
        else if (currentScreen == Screen.Win)
        {
            ProcessWinOptions(input);
        }
        
    }

    private void ProcessWinOptions(string input)
    {
        if(input != null)
        {
            ShowMainMenuScreen();
            currentScreen = Screen.MainMenu;
        }
    }

    public void ProcessInstructionsOptions()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ShowMainMenuScreen();
            currentScreen = Screen.MainMenu;
        }
    }

    void ProcessMainMenuOptions(string input)
    {
        if (input == "1" || input == "2" || input == "3")
        {
            level = int.Parse(input);
            currentScreen = Screen.Password;
            LoadPassword(input);
            StartGame();
        }
        else if (input == "007")
        {
            Terminal.WriteLine("Good Evening, Mr. Bond");
        }
        else if (input == "69")
        {
            Terminal.WriteLine("Niiiice");
        }
        else if (input == "menu")
        {
            ShowMainMenuScreen();
            currentScreen = Screen.MainMenu;
        }
        else
        {
            Terminal.WriteLine("Please choose a valid level");
        }
    }

    void LoadPassword(string input)
    {
        int passwordSlot;

        switch (input)
        {
            case "1":
                passwordSlot = Random.Range(0, level1Passwords.Length);
                password = level1Passwords[passwordSlot];
                break;
            case "2":
                passwordSlot = Random.Range(0, level2Passwords.Length);
                password = level2Passwords[passwordSlot];
                break;
            case "3":
                passwordSlot = Random.Range(0, level3Passwords.Length);
                password = level3Passwords[passwordSlot];
                break;
        }
    }

    private void ProcessPasswordGuess(string input)
    {
        if (input == "menu")
        {
            ShowMainMenuScreen();
            currentScreen = Screen.MainMenu;
        }
        else if (input == password)
        {
            Terminal.ClearScreen();
            currentScreen = Screen.Win;
            switch (level)
            {
                case 1:
                    ShowPoliceWinScreen();
                    break;
                case 2:
                    ShowSHPWinScreen();
                    break;
                case 3:
                    ShowFBIWinScreen();
                    break;
            }
        }
        else
        {
            Terminal.WriteLine("Password incorrect...please try again");
        }

    }

    //functions for early in the game, but afterwards aren't used again
    private void StartGame()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("You have chosen level " + level);
        scrambledPassword = ScrambleWord(password);
        if(scrambledPassword == password) { scrambledPassword = ScrambleWord(password); } //Had to add a check due to some shorter words coming back unscrambled
        Terminal.WriteLine("47.pwHlpr() returned " + scrambledPassword);
        Terminal.WriteLine("");
        Terminal.WriteLine("Please enter your password");
    }

    public void ShowFirstScreen()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("Hello Agent 47");
        Terminal.WriteLine("");
        Terminal.WriteLine("Your mission, should you choose to ");
        Terminal.WriteLine("accept it, is to hack into one of 3");
        Terminal.WriteLine("databases and discover intel that may");
        Terminal.WriteLine("prove useful to our operatives.");
        Terminal.WriteLine("");
        Terminal.WriteLine("Be warned, each level has a harder set of encryption.");
        Terminal.WriteLine("");
        Terminal.WriteLine("But with risk, comes great reward...");
    }

    public void ShowMainMenuScreen()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("");
        Terminal.WriteLine("1 - Local Police Station");
        Terminal.WriteLine("2 - State Highway Patrol Headquarters");
        Terminal.WriteLine("3 - FBI Criminal Database");
        Terminal.WriteLine("");
        Terminal.WriteLine("Enter the ID of the chosen database to continue:");
    }

    public void ShowPoliceWinScreen()
    {
        Terminal.WriteLine(@"
         ,   /\   ,
        / '-'  '-' \
       |   POLICE  |
        \  .---.  /
        | ( 121 ) |
        \  '---'  /
         '--. .--'
             '");
        Terminal.WriteLine("Police Database Accessed");
        Terminal.WriteLine("Good Work Agent 47");
        Terminal.WriteLine("Press 'enter' to play again");
    }

    public void ShowSHPWinScreen()
    {
        Terminal.WriteLine(@"
..................................
    _*_              _*_ 
 __/_|_\__ ---    __/_|_\__ ---
[(o)_R_(o)] ---  [(o)_R_(o)] ---
..................................");
        Terminal.WriteLine("");
        Terminal.WriteLine("State Highway System Accessed");
        Terminal.WriteLine("Good Work Agent 47");
        Terminal.WriteLine("Press 'enter' to play again");
    }

    public void ShowFBIWinScreen()
    {
        Terminal.WriteLine(@"
**********************
  ===== ====  ======
  ||    || \\   ||
  ||==  ||=<<   ||
  ||    || //   ||
  ==    ====  ======
**********************");
        Terminal.WriteLine("");
        Terminal.WriteLine("FBI Mainframe Accessed");
        Terminal.WriteLine("Good Work Agent 47");
        Terminal.WriteLine("Press 'enter' to play again");
    }

    public string ScrambleWord(string word)
    {
        char[] chars = new char[word.Length];
        system.Random rand = new system.Random();
        int index = 0;
        while (word.Length > 0)
        { // Get a random number between 0 and the length of the word. 
            int next = rand.Next(0, word.Length - 1); // Take the character from the random position and add to our char array.
            
            chars[index] = word[next];                // Remove the character from the word. 
            word = word.Substring(0, next) + word.Substring(next + 1);
            ++index;
        }
        return new system.String(chars);
    }
}
