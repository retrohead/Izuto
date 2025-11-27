![Izuto Logo](https://github.com/retrohead/Izuto/raw/main/_src/Izuto/Resources/IzutoLogo1.png)

# Izuto
Izuto is a tool to help with translating Inazuma Eleven 123

Built with .NET8.0 

Utilises parts of the code from of other tools developed by the community:

https://github.com/retrohead/Kuriimu2/
https://github.com/FanTranslatorsInternational/Kuriimu2


## Download:
Latest stable version usually compiled into the repo here:
https://github.com/retrohead/Izuto/tree/main/Izuto

## Getting Started

- First you should load the included izuto_options.json file which will pre-load our custom font and configuration used to manipulate the game to read more characters.
- Dump your rom file and extract the contents. Izuto supports opening files with the extension `.fa` found in level games.
- Translate the strings found in the files and update the `.fa` file using Izuto.
- Repackage your ROM file and test.

## Settings File

The settings file is used for 2 purposes:

- Replacing within the `.fa` file with new files. For example a custom font.
- Squeezing more characters into a smaller amount of bytes by adding custom syllables which should be swapped for Japanese characters.

Any modifications to the text translation listing should also be reflected in your custom font file.

## How to compile:
`git clone --recursive https://github.com/retrohead/Izuto.git`

If you already cloned without --recursive
`cd Izuto
git submodule update --init --recursive`