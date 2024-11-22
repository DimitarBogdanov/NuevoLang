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
    
    [Test]
    public void SingleString_WellFormed()
    {
        // Arrange
        const string source = "\"nuevo\"";
        Tokenizer tokenizer = new(source);
        Token[] tokens;
        
        // Act
        tokenizer.Tokenize();
        tokens = tokenizer.GetTokens();

        // Assert
        Assert.That(tokens, Has.Length.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LitString));
            Assert.That(tokens[0].Value, Is.EqualTo("nuevo"));
        });
    }
    
    [Test]
    public void OperatorsBasic()
    {
        // Arrange
        const string source = "+ - * / % ^ && || ! = == != < <= > >=";
        Tokenizer tokenizer = new(source);
        Token[] tokens;
        
        // Act
        tokenizer.Tokenize();
        tokens = tokenizer.GetTokens();

        // Assert
        Assert.That(tokens, Has.Length.EqualTo(16));
        Assert.Multiple(() =>
        {
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.OpAdd));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpSub));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.OpMul));
            Assert.That(tokens[3].Type, Is.EqualTo(TokenType.OpDiv));
            Assert.That(tokens[4].Type, Is.EqualTo(TokenType.OpMod));
            Assert.That(tokens[5].Type, Is.EqualTo(TokenType.OpPow));
            Assert.That(tokens[6].Type, Is.EqualTo(TokenType.OpAnd));
            Assert.That(tokens[7].Type, Is.EqualTo(TokenType.OpOr));
            Assert.That(tokens[8].Type, Is.EqualTo(TokenType.OpNot));
            Assert.That(tokens[9].Type, Is.EqualTo(TokenType.OpAssign));
            Assert.That(tokens[10].Type, Is.EqualTo(TokenType.OpEq));
            Assert.That(tokens[11].Type, Is.EqualTo(TokenType.OpNEq));
            Assert.That(tokens[12].Type, Is.EqualTo(TokenType.OpLess));
            Assert.That(tokens[13].Type, Is.EqualTo(TokenType.OpLessEq));
            Assert.That(tokens[14].Type, Is.EqualTo(TokenType.OpGreater));
            Assert.That(tokens[15].Type, Is.EqualTo(TokenType.OpGreaterEq));
        });
    }
    
    [Test]
    public void OperatorsCompound()
    {
        // Arrange
        const string source = "+= -= *= /= %= ^=";
        Tokenizer tokenizer = new(source);
        Token[] tokens;
        
        // Act
        tokenizer.Tokenize();
        tokens = tokenizer.GetTokens();

        // Assert
        Assert.That(tokens, Has.Length.EqualTo(6));
        Assert.Multiple(() =>
        {
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.OpAssignAdd));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpAssignSub));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.OpAssignMul));
            Assert.That(tokens[3].Type, Is.EqualTo(TokenType.OpAssignDiv));
            Assert.That(tokens[4].Type, Is.EqualTo(TokenType.OpAssignMod));
            Assert.That(tokens[5].Type, Is.EqualTo(TokenType.OpAssignPow));
        });
    }
    
    [Test]
    public void ModuleDef()
    {
        // Arrange
        const string source = "module :: App";
        Tokenizer tokenizer = new(source);
        Token[] tokens;
        
        // Act
        tokenizer.Tokenize();
        tokens = tokenizer.GetTokens();

        // Assert
        Assert.That(tokens, Has.Length.EqualTo(3));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.KwModule));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpDoubleColon));
        Assert.Multiple(() =>
        {
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Id));
            Assert.That(tokens[2].Value, Is.EqualTo("App"));
        });
    }
    
    [Test]
    public void FunctionDef()
    {
        // Arrange
        const string source = "function(){}";
        Tokenizer tokenizer = new(source);
        Token[] tokens;
        
        // Act
        tokenizer.Tokenize();
        tokens = tokenizer.GetTokens();

        // Assert
        Assert.That(tokens, Has.Length.EqualTo(5));
        Assert.Multiple(() =>
        {
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.KwFunction));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.ParenLeft));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.ParenRight));
            Assert.That(tokens[3].Type, Is.EqualTo(TokenType.BraceLeft));
            Assert.That(tokens[4].Type, Is.EqualTo(TokenType.BraceRight));
        });
    }
}