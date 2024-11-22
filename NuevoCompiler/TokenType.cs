namespace NuevoCompiler;

public enum TokenType
{
    Id,
    LitNum,
    LitBool,
    LitString,
    LitNull,
    
    KwModule,
    KwFunction,
    KwIf,
    KwElseIf,
    KwElse,
    KwFor,
    KwReturn,
    KwWhile,
    KwHandle,
    KwHandleCase,
    
    ParenLeft,
    ParenRight,
    BraceLeft,
    BraceRight,
    BracketLeft,
    BracketRight,
    
    OpDoubleColon,
    OpComma,
    OpEq,
    OpNEq,
    OpLess,
    OpGreater,
    OpLessEq,
    OpGreaterEq,
    
    OpAdd,
    OpSub,
    OpMul,
    OpDiv,
    OpMod,
    OpPow,
    
    OpAssign,
    OpAssignAdd,
    OpAssignSub,
    OpAssignMul,
    OpAssignDiv,
    OpAssignMod,
    OpAssignPow,
    
    OpAnd,
    OpOr,
    OpNot,
    OpLength,
}