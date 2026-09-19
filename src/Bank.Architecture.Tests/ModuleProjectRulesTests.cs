using System.Xml.Linq;
using Xunit;

namespace Bank.Architecture.Tests;

public sealed class ModuleProjectRulesTests
{
    private static readonly string SourceRoot = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

    private static readonly string ModulesRoot = Path.GetFullPath(
        Path.Combine(SourceRoot, "Modules"));

    private static readonly string ContractsProject = Path.Combine(
        SourceRoot, "Bank.Contracts", "Bank.Contracts.csproj");

    private static readonly string[] ModuleLayers =
    [
        ".Domain.csproj",
        ".Application.csproj",
        ".Infrastructure.csproj",
        ".Presentation.csproj"
    ];

    [Fact]
    public void Every_module_has_all_four_layers()
    {
        var moduleDirectories = Directory.GetDirectories(ModulesRoot);

        Assert.NotEmpty(moduleDirectories);

        foreach (var moduleDirectory in moduleDirectories)
        {
            var moduleName = Path.GetFileName(moduleDirectory);
            foreach (var layer in ModuleLayers)
            {
                var projectFile = $"Bank.{moduleName}{layer}";
                Assert.True(
                    File.Exists(Path.Combine(moduleDirectory, Path.GetFileNameWithoutExtension(projectFile), projectFile)),
                    $"{moduleName} is missing its {layer.TrimStart('.').Replace(".csproj", string.Empty)} project.");
            }
        }
    }

    [Fact]
    public void Module_projects_reference_only_their_own_module_or_shared_contracts()
    {
        var projectFiles = Directory.GetFiles(ModulesRoot, "*.csproj", SearchOption.AllDirectories);

        foreach (var projectFile in projectFiles)
        {
            var projectName = Path.GetFileNameWithoutExtension(projectFile);
            var moduleDirectory = Directory.GetParent(projectFile)!.Parent!.FullName;
            var ownModuleRoot = Path.GetFullPath(moduleDirectory);
            var references = XDocument.Load(projectFile)
                .Descendants("ProjectReference")
                .Select(reference => reference.Attribute("Include")?.Value)
                .Where(reference => reference is not null)
                .Select(reference => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(projectFile)!, reference!)))
                .Select(Path.GetFullPath)
                .ToArray();

            var forbiddenModuleReferences = references
                .Where(reference => reference.Contains(
                    $"{Path.DirectorySeparatorChar}Modules{Path.DirectorySeparatorChar}",
                    StringComparison.OrdinalIgnoreCase))
                .Where(reference => !reference.StartsWith(
                    ownModuleRoot + Path.DirectorySeparatorChar,
                    StringComparison.OrdinalIgnoreCase))
                .ToArray();

            Assert.True(
                forbiddenModuleReferences.Length == 0,
                $"{projectName} references another module: " +
                string.Join(", ", forbiddenModuleReferences));

            var forbiddenLegacyReferences = references
                .Where(reference =>
                    reference.EndsWith("Bank.Domain.csproj", StringComparison.OrdinalIgnoreCase) ||
                    reference.EndsWith("Bank.Application.csproj", StringComparison.OrdinalIgnoreCase) ||
                    reference.EndsWith("Bank.Infrastructure.csproj", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            Assert.True(
                forbiddenLegacyReferences.Length == 0,
                $"{projectName} references a legacy horizontal project: " +
                string.Join(", ", forbiddenLegacyReferences));
        }
    }

    [Fact]
    public void Contracts_do_not_reference_modules()
    {
        var moduleReferences = XDocument.Load(ContractsProject)
            .Descendants("ProjectReference")
            .Select(reference => reference.Attribute("Include")?.Value)
            .Where(reference => reference is not null && reference.Contains("Modules", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.Empty(moduleReferences);
    }
}
