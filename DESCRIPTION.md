# Description

Create general editors for 2D tiled games.

## Philosophy

The purpose of this project is to provide a configurable palette randomize for
The Legend of Zelda: A Link to the Past. I built this because the current
randomizer did not account for certain colors existing in many different
palette regions and you would get some tiles like grass or or water having
different colors and looking disconnected.

## Open Source

This project is being built with the goal of being completely open source, and 
enforcing all programs that use it be open source too. Open source allows
anyone to contribute, fix, and improve the project at any time. It also allows 
anyone to use the project for their own purposes.

For more information on the value of open source, read the
[Open Source Guide](https://opensource.guide/).

## Cross platform

In conjunction with open source, being cross platform is an essential component 
of this project; it should be accessible to anyone on any OS. Making
cross-platform projects also becomes easier for open source projects where
users with different setups can contribute.

## Full documentation

Sometimes, the biggest challenge of contributing to a new project is
understanding what all of the code does. For this reason, it is our philosophy 
that every function, module, and component be documented and outlined, and have 
relevant example code to show users how its used and why. We utilize the 
[Sandcastle Help File Builder (SHFB)][sfhb] to build the documentation. SHFB
uses formatted comments in source code files to build documentation pages in
many formats, including HTML, Markdown, and Windows Help File.

## Community driven environment

Please follow the [Code of Conduct](CODE_OF_CONDUCT.md) for a more detailed 
explanation of working in a community project.

## Accessibility

Follow the guideilines provided in Microsoft's
[Globalization and localizing .NET applications][globalization]. As a
quick and general rule, be sure to

- Use resource files in place of hardcoded strings.
- Use culture-specific settings where appropriate.
- Handle dates and times according to the user's locale.

[sfhb]: https://github.com/EWSoftware/SHFB/releases
[globalization]: https://docs.microsoft.com/en-us/dotnet/standard/globalization-localization/
