# DiamondFlasher

A simple console application to use as flashcards.

![](https://github.com/Biegus/DiamondFlasher/blob/main/Images/Main.png)

## Download executable

You can download the executable from here https://github.com/Biegus/DiamondFlasher/releases/tag/1.0

## Basic rules

You are (almost) always in certain context. You can check commands available by typing *help*

All arguments should by wrap in *"*

Arguments that description starts with *?* are optional

## How to use

1. First of all, you need to load some flashcards. There are various ways of doing it.

-Use *parse_raw* and then type or past all flashcards. Split keys and values with "-". Empty line means end.

-Use *load_file*. In that case you have to specify the file name. Optionally you can change splitter ("-" by default).

-Use *load_all_file* or *load_file_desktop*, they are much more specfied ways for loading files.

2. When you are done adding, if you want to invert keys and values you can use *invert*

3. Now, you can start session by typing *start_session*.

4. After session ends, you are in the **EndOfSessionContext**. If you want to go back, add some flashcards, or just start session again, type *exit_context*.
However, you can do some interesting stuff while in **EndOfSessionContext**:
You can unload words, that you got right first time by using *unload_perfects*. You can also start specialized session, with only words that made you struggle by *next_without_perfects*.

## Credits

Created and programmed by Biegus (Michał Biegaj). 
Thanks for the  TheMatiaz0 for helping with small language issues
