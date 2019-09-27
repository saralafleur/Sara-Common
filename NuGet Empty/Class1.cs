namespace NuGet_Empty
{
    /// <summary>
    /// Concern
    ///   When targeting multiple frameworks (i.e. .NET Framework, .NET Standard, etc),
    ///   the Post Build Event will run for for each framework.  Causing the
    ///   Post Build Event to run multiple times.
    /// 
    ///   I need it to only run once to pack the 'Sara-Common.nupkg'
    /// 
    /// Solution
    ///   Use an Empty Project's Post Build Event.
    ///
    /// Steps
    ///   1.) Perform the following for each Project you want to include in the NuGet Package
    ///     a.) Add the Project as a dependency
    ///     b.) Add the Project as a Reference - This will force a recompile each time you build 'Nuget Empty'.
    ///   2.) Add the Post Build Event to pack 'Sara-Common.nuspec'
    /// </summary>
    public class Class1
    {
    }
}
