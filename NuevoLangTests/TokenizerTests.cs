using System.Diagnostics.CodeAnalysis;
using NuevoCompiler;

namespace NuevoLangTests;

[SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void EmptySource()
    {
        // Arrange
        const string source = "";
        Tokenizer tokenizer = new(source);
        Token[] tokens;
        
        // Act
        tokenizer.Tokenize();
        tokens = tokenizer.GetTokens();

        // Assert
        Assert.That(tokens, Is.Empty);
    }

    [Test]
    public void SingleIdentifier()
    {
        // Arrange
        const string source = "nuevo";
        Tokenizer tokenizer = new(source);
        Token[] tokens;
        
        // Act
        tokenizer.Tokenize();
        tokens = tokenizer.GetTokens();

        // Assert
        Assert.That(tokens, Has.Length.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Id));
            Assert.That(tokens[0].Value, Is.EqualTo("nuevo"));
        });
    }
}