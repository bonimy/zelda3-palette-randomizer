# Link to the Past Palette Randomizer

C# .NET Console application to randomize palettes in The Legend of Zelda: A
Link to the Past.

## Table of Contents

- [What is a palette randomizer?](#what-is-a-palette-randomizer)
- [How to use](#how-to-use)
- [Building from source](#building-from-source)
    - [Visual Studio](#visual-studio)
- [Contributions](#contributions)
- [Credits](#credits)
- [License](#license)

## What is a palette randomizer?

A palette randomizer is, as the name suggests, something that randomizes a
game's _palettes_ (its color collections). This randomizer is designed to
randomize colors according to their groupings. For example, all colors that
represent grass will be randomized with the same logic, whereas water's colors
will be randomized with different logic. This creates a consistent color scheme
without any disconnects in colors (e.g. grass having two different colors).

A set of colors is randomized according to the following rules:
- Shift a color's hue by at least 5%. This ensures a color is actually changed.
- Shift a color's saturation/chroma by no more than 50% of its original. This
  prevents colors from looking over or undersaturated compared to the original,
  which is cause for many ugly colors.
- Shift a color's luma/lightness by no more than 25% of its original. Lightness
  is even more sensitive than saturation and should not be altered too much.

I may make the percentages customizable.

![example1](https://cdn.discordapp.com/attachments/329059206030295051/641420281608405022/unknown.png)
![example2](https://cdn.discordapp.com/attachments/329059206030295051/641445510074466304/unknown.png)

## How to use

Go to our [Releases](https://github.com/bonimy/zelda3-palette-randomizer/releases)
page. From there, download _Z3PaletteRandomizer.zip_ under that _Assets_ tab of
the latest release.

You can simply drag and drop the Link to the Past Randomized ROM you wish to use
onto the application `MushROMs.exe`. Or you can start the app yourself and
manually enter the path to the ROM.

It is recommended that your logic randomizer app keep the vanilla palette, or
you may still experience ugly colors after using this app.

## Building from source

Presently, the [Visual Studio 2017 IDE][vs17] is the only supported
environment. Users are encouraged to suggest new environments in our
[Issues][issues] section.

### Visual Studio
- Get the [latest][vs_latest] version of Visual Studio. At the time of writing 
  this, it should be Visual Studio 2017. You have three options:
  [Community, Professional, and Enterprise][vs_compare]. Any of these three are 
  fine. The collaborators presently build against community since it is free.
  See that you meet the [System Requirements][vs_req] for Visual Studio for
  best interaction.
- When installing Visual Studio, under the _Workloads_ tab, select
**.NET desktop development**. Under the _Individual Components_ tab, select
  .NET Framework 4.7 SDK and .NET Framework 4.7 targeting pack if they weren't
  already selected. If a newer SDK is available, be sure to select that one as 
  well. We will keep up to date with the SDK as frequently as possible.
- Click Install and let the installer do it's thing.
- Clone our repository and open the solution file in Visual Studio.
- Open the test explorer and make sure all tests are passing.

## Contributions

Do you want to add a feature, report a bug, or propose a change to the
project? That's awesome! First, please refer to our
[Contributing](CONTRIBUTING.md) file. We use it in hopes having the best
working environment we can.

## Credits

* [Nelson Garcia](https://github.com/bonimy): Project leader and main
programmer

## License

C# .NET Console App for randomizing Link to the Past palettes
Copyright (C) 2018-2020 Nelson Garcia

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program. If not, see http://www.gnu.org/licenses/.

[vs17]: https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes
[issues]: https://github.com/Maseya/Editors/issues
[vs_latest]: https://www.visualstudio.com/downloads
[vs_compare]: https://www.visualstudio.com/vs/compare
[vs_req]: https://www.visualstudio.com/en-us/productinfo/vs2017-system-requirements-vs
