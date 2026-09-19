using System.Xml.Linq;

namespace Bank.Architecture.Tests;

public sealed class ModuleProjectRulesTests
{
    private static readonly string ModulesRoot = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Modules"));

    [Fact]
    public void Module_projects_do_not_reference_another_modules_domain_or_infrastructure()
    {
        var projectFiles = Directory.GetFiles(ModulesRoot, "*.csproj", SearchOption.AllDirectories);

        foreach (var projectFile in projectFiles)
        {
            var projectName = Path.GetFileNameWithoutExtension(projectFile);
            var moduleDirectory = Directory.GetParent(projectFile)!.Parent!.FullName;
            var references = XDocument.Load(projectFile)
                .Descendants("ProjectReference")
                .Select(reference => reference.Attribute("Include")?.Value)
                .Where(reference => reference is not null)
                .Select(reference => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(projectFile)!, reference!)))
                .Select(Path.GetFullPath)
                .ToArray();

            var forbiddenReferences = references
                .Where(reference => reference.Contains($"{Path.DirectorySeparatorChar}Modules{Path.DirectorySeparatorChar}"))
                .Where(reference => !reference.StartsWith(moduleDirectory, StringComparison.OrdinalIgnoreCase))
                .Where(reference =>
                    reference.EndsWith(".Domain.csproj", StringComparison.OrdinalIgnoreCase) ||
                    reference.EndsWith(".Infrastructure.csproj", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            Assert.True(
                forbiddenReferences.Length == 0,
                $"{projectName} references another module's Domain or Infrastructure project: " +
                string.Join(", ", forbiddenReferences));
        }
    }

    [Fact]
    public void Contracts_do_not_reference_modules()
    {
        var contractsProject = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Bank.Contracts", "Bank.Contracts.csproj"));

        var moduleReferences = XDocument.Load(contractsProject)
            .Descendants("ProjectReference")
            .Select(reference => reference.Attribute("Include")?.Value)
            .Where(reference => reference is not null && reference.Contains("Modules", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.Empty(moduleReferences);
    }
}