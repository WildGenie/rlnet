# RLNET #

RLNET is a lightweight API to help quickly create tile-based games for .NET. RLNET provides a console output, swappable colors, true color sprites, keyboard and mouse inputs.



RLNET uses [OpenTK](http://www.opentk.com)

# Getting Started #

Download the latest version and add a reference to RLNET.dll. Be sure to also add OpenTK.dll to the debug and release folders of your project.

# Using RLNET #

First you must create your root console. This object has access to the window, keyboard, and mouse to for your game.
```
RLRootConsole rootConsole = new RLRootConsole("terminal8x8.png", 50, 50, 8, 8, 1f, "RLNET Sample");
```

Then you hook up to its update and render events.

```
rootConsole.Update += rootConsole_Update;
rootConsole.Render += rootConsole_Render;

...

void rootConsole_Update(object sender, UpdateEventArgs e)
{
    //Update Logic Here
}

void rootConsole_Render(object sender, UpdateEventArgs e)
{
    rootConsole.Clear();
    rootConsole.Print(1, 1, "Hello World!", RLColor.White);
    rootConsole.Draw();
}
```

Finally you let it run.
```
rootConsole.Run();
```

Take a look at the RLNET.Sample project to see a working sample.

# License #
RLNET is released under the [MIT License](http://en.wikipedia.org/wiki/MIT_License)

Copyright (c) 2014 Travis M. Clark

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to 
deal in the Software without restriction, including without limitation the 
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in 
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.