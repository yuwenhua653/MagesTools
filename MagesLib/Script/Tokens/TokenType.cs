namespace Mages.Script.Tokens
{
    public enum TokenType
    {
        LineBreak = 0x0,
        CharacterNameStart = 0x1,
        DialogueLineStart = 0x2,
        Present = 0x3,
        SetColor = 0x4,
        Present_ResetAlignment = 0x8,
        RubyBaseStart = 0x9,
        RubyTextStart = 0xA,
        RubyTextEnd = 0xB,
        SetFontSize = 0xC,
        PrintInParallel = 0xE,
        CenterText = 0xF,
        SetTopMargin = 0x11,
        SetLeftMargin = 0x12,
        GetHardcodedValue = 0x13,
        EvaluateExpression = 0x15,
        Present_0x18 = 0x18,
        AutoForward = 0x19,
        AutoForward_1A = 0x1A,
        Unk_1E = 0x1E,
        AltLineBreak = 0x1F,

        TextMask = 0x80,
        Terminator = 0xFF,
    }
}
