# Getting up to speed again

The tutorial I was following is [here](https://rimworldwiki.com/wiki/Plague_Gun_(1.1)).

## Being able to build it
1. Check the csproj to work out what .NET version to install, and install the SDK if necessary
2. Install Visual Studio Community Edition (Just c# desktop development, no need for extras)
3. When you open the BloodDries solution, you might need to add the refrences for the RimWorld dlls again (depending on where Steam is installed and this repo has been pulled to).

## Dev setup
* Use dnSpy to explore the `Assembly-CSharp.dll`, and export it under `file`, then open with Visual Studio (so that you can find references and click through explore)
* Open up the `RimWorld\Data\Core\Defs` folder in VSCode
* Open up the mod solution in Visual Studio.


## Testing
I can't remember how I debugged it before, but building the project in debug so the assembly gets updated, then copying the whole folder into the `Rimworld/Mods` folder should allow you to test it.


# Where I go to
* Looking at `Thing.cs`, there is a method called `Notify_ColorChanged()`, and looking at usages, the Bed seems to use it when changing designation.  And it uses the `DrawColor` property.  I wonder if I can override that method.
* I also need to work out how to tell what the age of the blood is, so that I can calculate the color..
* There seems to be a tick type, that I might be able to use, along with the `Tick()` method override, to call `Notify_ColorChanged()` (if that is necessary).
