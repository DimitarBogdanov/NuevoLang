using System.Text;

namespace NuevoCompiler;

/// <summary>
/// The tokenizer splits the input source into an array of <see cref="Token"/>.
/// </summary>
public sealed class Tokenizer
{
    private enum State
    {
        Default,
        String,
        Num
    }

    public Tokenizer(string source)
    {
        _reader = new StringReader(source);
        _tokens = new LinkedList<Token>();
        _value = new StringBuilder();
    }

    private readonly StringReader      _reader;
    private readonly LinkedList<Token> _tokens;
    private readonly StringBuilder     _value;

    private State _state;

    public Token[] GetTokens()
    {
        return _tokens.ToArray();
    }

    public void Tokenize()
    {
        while (_reader.Peek() > 0)
        {
            char current = (char)_reader.Read();
            char next = (char)_reader.Peek();

            restart:
            if (_state == State.String)
            {
                if (current == '"')
                {
                    PushTokAndResetState();
                    continue;
                }

                _value.Append(current);
                continue;
            }

            if (_state == State.Num)
            {
                if (Char.IsDigit(current))
                {
                    _value.Append(current);
                }
                else if (current == '.')
                {
                    // TODO: Error when having multiple periods
                    _value.Append('.');
                }
                else
                {
                    PushTokAndResetState();
                    goto restart;
                }
            }
            else if (Char.IsDigit(current))
            {
                PushTokAndResetState();

                _state = State.Num;
                _value.Append(current);
            }

            // Note: state == Default

            if (current == '"')
            {
                PushTokAndResetState();
                _state = State.String;
                continue;
            }

            switch (current)
            {
                case '+':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpAssignAdd);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpAdd);
                    }

                    continue;
                }

                case '-':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpAssignSub);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpSub);
                    }

                    continue;
                }

                case '*':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpAssignMul);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpMul);
                    }

                    continue;
                }

                case '/':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpAssignDiv);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpDiv);
                    }

                    continue;
                }

                case '%':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpAssignMod);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpMod);
                    }

                    continue;
                }

                case '^':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpAssignPow);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpPow);
                    }

                    continue;
                }

                case '=':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpEq);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpAssign);
                    }

                    continue;
                }

                case '!':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpNEq);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpNot);
                    }

                    continue;
                }

                case '<':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpLessEq);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpLess);
                    }

                    continue;
                }

                case '>':
                {
                    if (next == '=')
                    {
                        _reader.Read();
                        QuickAddTok(TokenType.OpGreaterEq);
                    }
                    else
                    {
                        QuickAddTok(TokenType.OpGreater);
                    }

                    continue;
                }

                case '&' when next == '&':
                {
                    _reader.Read();
                    QuickAddTok(TokenType.OpAnd);
                    continue;
                }

                case '|' when next == '|':
                {
                    _reader.Read();
                    QuickAddTok(TokenType.OpOr);
                    continue;
                }
                
                case ':' when next == ':':
                {
                    _reader.Read();
                    QuickAddTok(TokenType.OpDoubleColon);
                    continue;
                }

                case ',':
                {
                    QuickAddTok(TokenType.OpComma);
                    continue;
                }

                case '#':
                {
                    QuickAddTok(TokenType.OpLength);
                    continue;
                }
                
                case '(':
                {
                    QuickAddTok(TokenType.ParenLeft);
                    continue;
                }
                
                case ')':
                {
                    QuickAddTok(TokenType.ParenRight);
                    continue;
                }
                
                case '{':
                {
                    QuickAddTok(TokenType.BraceLeft);
                    continue;
                }
                
                case '}':
                {
                    QuickAddTok(TokenType.BraceRight);
                    continue;
                }
                
                case '[':
                {
                    QuickAddTok(TokenType.BracketLeft);
                    continue;
                }
                
                case ']':
                {
                    QuickAddTok(TokenType.BracketRight);
                    continue;
                }

                default:
                {
                    if (Char.IsWhiteSpace(current))
                    {
                        PushTokAndResetState();
                    }
                    else if (Char.IsLetter(current)
                        || current == '_'
                        || (_value.Length != 0 && Char.IsDigit(current))
                       )
                    {
                        _value.Append(current);
                        continue;
                    }

                    // TODO: Error - unknown char in identifier
                    break;
                }
            }
        }
        
        PushTokAndResetState();
    }

    /// <summary>
    /// Adds a token to the list with an empty value and the given <see cref="TokenType"/>.
    /// </summary>
    private void QuickAddTok(TokenType tok)
    {
        _tokens.AddLast(new Token(tok, ""));
    }

    /// <summary>
    /// Pushes the current token and clears the state of the tokenizer.
    /// The type of the token is inferred by its value.
    /// </summary>
    private void PushTokAndResetState()
    {
        switch (_state)
        {
            case State.Default:
            {
                string val = _value.ToString();
                if (val == "")
                {
                    ResetStateFull();
                    return;
                }

                TokenType type = val switch
                {
                    "true"  => TokenType.LitBool,
                    "false" => TokenType.LitBool,
                    "null"  => TokenType.LitNull,

                    "module"   => TokenType.KwModule,
                    "function" => TokenType.KwFunction,
                    "if"       => TokenType.KwIf,
                    "else"     => TokenType.KwElse,
                    "elseif"   => TokenType.KwElseIf,
                    "for"      => TokenType.KwFor,
                    "return"   => TokenType.KwReturn,
                    "while"    => TokenType.KwWhile,
                    "handle"   => TokenType.KwHandle,
                    "ok"       => TokenType.KwHandleCase,

                    _ => TokenType.Id
                };

                _tokens.AddLast(new Token(type, val));

                break;
            }

            case State.String:
            {
                string val = _value.ToString();
                Token tok = new(TokenType.LitString, val);
                _tokens.AddLast(tok);
                break;
            }

            case State.Num:
            {
                string val = _value.ToString();
                Token tok = new(TokenType.LitNum, val);
                _tokens.AddLast(tok);
                break;
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(_state));
        }

        ResetStateFull();
    }

    /// <summary>
    /// Resets the state of the tokenizer.
    /// </summary>
    private void ResetStateFull()
    {
        _state = State.Default;
        _value.Clear();
    }
}