# Bejeweled Neon

#### *Simple guide.*

This guide is a simplified version of an documentation, they will contain the core information's about the project.

![](https://i.imgur.com/TbaHLUz.gif)

### Game Manager
    The Game Manager is a singleton used to control all the flow over the game, here all game required information and classes will be found, 
    have his initialization and monobehaviour calls. All fields are self descriptive (i hope).

  
![](https://i.imgur.com/L9W5At7.png)

### Gem and Collections

    All gems cames fron an scriptable object data, in theory you can create any kind of new gem you want.

    A collection is a scriptable object that have a list of gems, this collection is loaded by the Game Manger and passed to game board and
    all this information will be used to generate the in-game gems.
#
    Creating Gem Collection or Gem Data.
![](https://i.imgur.com/pRqDpxp.png)
#
    Gem Data Sample
![](https://i.imgur.com/qMqiUIp.png)
#
    Gem Collection Sample
![](https://i.imgur.com/iFilTZC.png)

