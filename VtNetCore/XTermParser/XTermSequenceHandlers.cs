﻿namespace VtNetCore.XTermParser
{
    using System;
    using System.Linq;
    using VtNetCore.Exceptions;
    using VtNetCore.VirtualTerminal;
    using VtNetCore.VirtualTerminal.Enums;
    using VtNetCore.XTermParser.SequenceType;

    public static class XTermSequenceHandlers
    {
        public static SequenceHandler[] Handlers =
        {
            new SequenceHandler
            {
                SequenceType = SequenceHandler.ESequenceType.Character,
                Handler = (sequence, controller) =>
                {
                    var characterSequence = sequence as CharacterSequence;
                    switch(characterSequence.Character)
                    {
                        case '\n':
                            controller.NewLine();
                            break;
                        case '\r':
                            controller.CarriageReturn();
                            break;
                        case '\b':
                            controller.Backspace();
                            break;
                        case '\u0007':
                            controller.Bell();
                            break;
                        case '\t':
                            controller.Tab();
                            break;
                        case '\u000F':
                            controller.ShiftIn();
                            break;
                        case '\u000E':
                            controller.ShiftOut();
                            break;
                        case '\u000B':
                            controller.VerticalTab();
                            break;
                        case '\f':
                            controller.FormFeed();
                            break;
                        default:
                            controller.PutChar(characterSequence.Character);
                            break;
                    }
                }
            },
            new SequenceHandler
            {
                SequenceType = SequenceHandler.ESequenceType.CharacterSet,
                Handler = (sequence, controller) =>
                    controller.SetCharacterSet(
                        (sequence as CharacterSetSequence).CharacterSet,
                        (sequence as CharacterSetSequence).Mode
                        )
            },
            new SequenceHandler
            {
                Description = "Save Cursor (DECSC)",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "7",
                Handler = (sequence, controller) => controller.SaveCursor()
            },
            new SequenceHandler
            {
                Description = "Restore Cursor (DECRC).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "8",
                Handler = (sequence, controller) => controller.RestoreCursor()
            },
            new SequenceHandler
            {
                Description = "Application Keypad (DECKPAM)",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "=",
                Handler = (sequence, controller) => controller.SetKeypadType(EKeypadType.Application)
            },
            new SequenceHandler
            {
                Description = "Normal Keypad (DECKPNM).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = ">",
                Handler = (sequence, controller) => controller.SetKeypadType(EKeypadType.Normal)
            },
            new SequenceHandler
            {
                Description = "Full Reset (RIS)",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "c",
                Handler = (sequence, controller) => controller.FullReset()
            },
            new SequenceHandler
            {
                Description = "Index (IND  is 0x84).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "D",
                Handler = (sequence, controller) => controller.NewLine()
            },
            new SequenceHandler
            {
                Description = "Next Line (NEL  is 0x85).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "E",
                Handler = (sequence, controller) => {
                    controller.CarriageReturn();
                    controller.NewLine();
                }
            },
            new SequenceHandler
            {
                Description = "Tab Set",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "H",
                Handler = (sequence, controller) => controller.TabSet()
            },
            new SequenceHandler
            {
                Description = "Reverse Index",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "M",
                Handler = (sequence, controller) => controller.ReverseIndex()
            },
            new SequenceHandler
            {
                Description = "Invoke the G2 Character Set as GL (LS2).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "n",
                Handler = (sequence, controller) => controller.InvokeCharacterSetMode(ECharacterSetMode.IsoG2)
            },
            new SequenceHandler
            {
                Description = "Invoke the G3 Character Set as GL (LS3).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "o",
                Handler = (sequence, controller) => controller.InvokeCharacterSetMode(ECharacterSetMode.IsoG3)
            },
            new SequenceHandler
            {
                Description = "Invoke the G1 Character Set as GR (LS1R).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "~",
                Handler = (sequence, controller) => controller.InvokeCharacterSetModeR(ECharacterSetMode.IsoG1)
            },
            new SequenceHandler
            {
                Description = "Invoke the G2 Character Set as GR (LS2R).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "}",
                Handler = (sequence, controller) => controller.InvokeCharacterSetModeR(ECharacterSetMode.IsoG2)
            },
            new SequenceHandler
            {
                Description = "Invoke the G3 Character Set as GR (LS3R).",
                SequenceType = SequenceHandler.ESequenceType.Escape,
                CsiCommand = "|",
                Handler = (sequence, controller) => controller.InvokeCharacterSetModeR(ECharacterSetMode.IsoG3)
            },
            new SequenceHandler
            {
                Description = "DEC double-height line, top half (DECDHL).",
                SequenceType = SequenceHandler.ESequenceType.CharacterSize,
                Handler = (sequence, controller) => controller.SetCharacterSize((sequence as CharacterSizeSequence).Size)
            },
            new SequenceHandler
            {
                Description = "Set Text Parameters (Icon and Title)",
                SequenceType = SequenceHandler.ESequenceType.OSC,
                Param0 = new int [] { 0, 1, 2, 21 },
                Handler = (sequence, controller) => controller.SetWindowTitle(sequence.Command)
            },
            new SequenceHandler
            {
                Description = "Insert Ps (Blank) Character(s) (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "@",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => controller.InsertBlanks(((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]))
            },
            new SequenceHandler
            {
                Description = "Character Position Relative  [columns] (default = [row,col+1])",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "a",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.MoveCursorRelative(count, 0);
                }
            },
            new SequenceHandler
            {
                Description = "Send Device Attributes (Secondary DA).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                Send = true,
                CsiCommand = "c",
                ExactParameterCountOrDefault = 1,
                Param0 = new int [] { 0 },
                Handler = (sequence, controller) => controller.SendDeviceAttributesSecondary()
            },
            new SequenceHandler
            {
                Description = "Send Device Attributes (Primary DA).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "c",
                ExactParameterCountOrDefault = 1,
                Param0 = new int [] { 0 },
                Handler = (sequence, controller) => controller.SendDeviceAttributes()
            },
            new SequenceHandler
            {
                Description = "Line Position Absolute  [row] (default = [1,column])",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "d",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var value = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.SetAbsoluteRow(value);
                }
            },
            new SequenceHandler
            {
                Description = "Line Position Relative  [rows] (default = [row+1,column])",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "e",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.MoveCursorRelative(0, count);
                }
            },
            new SequenceHandler
            {
                Description = "Horizontal and Vertical Position [row;column]",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "f",
                ExactParameterCountOrDefault = 2,
                Handler = (sequence, controller) =>
                {
                    if(sequence.Parameters.Count == 0)
                        controller.SetCursorPosition(1,1);
                    else
                    {
                        var row = Math.Max(sequence.Parameters[0], 1);
                        var col = Math.Max(sequence.Parameters[1], 1);
                        controller.SetCursorPosition(col, row);
                    }
                }
            },
            new SequenceHandler
            {
                Description = "Tab Clear (TBC) - Clear Current Column (default).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "g",
                ExactParameterCountOrDefault = 1,
                Param0 = new int [] { 0 },
                Handler = (sequence, controller) => controller.ClearTab()
            },
            new SequenceHandler
            {
                Description = "Tab Clear (TBC) - Clear all.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "g",
                ExactParameterCount = 1,
                Param0 = new int [] { 3 },
                Handler = (sequence, controller) => controller.ClearTabs()
            },
            new SequenceHandler
            {
                Description = "Set Mode (SM).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "h",
                Handler = (sequence, controller) =>
                {
                    foreach(var parameter in sequence.Parameters)
                    {
                        switch(parameter)
                        {
                            case 4:     // Ps = 4  -> Insert Mode (IRM).
                                controller.SetInsertReplaceMode(EInsertReplaceMode.Insert);
                                break;

                            case 20:    // Ps = 2 0  -> Automatic Newline (LNM).
                                controller.SetAutomaticNewLine(true);
                                break;

                            default:
                                System.Diagnostics.Debug.WriteLine("Set Mode (SM) mode: " + parameter.ToString() + " is unknown");
                                break;
                        }
                    }
                }
            },
            new SequenceHandler
            {
                Description = "DEC Private Mode Set (DECSET).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "h",
                Query = true,
                Handler = (sequence, controller) =>
                {
                    foreach(var parameter in sequence.Parameters)
                    {
                        switch(parameter)
                        {
                            case 1:     // Ps = 1  -> Application Cursor Keys (DECCKM).
                                controller.EnableApplicationCursorKeys(true);
                                break;

                            case 3:     // Ps = 3  -> 132 Column Mode (DECCOLM).
                                controller.Enable132ColumnMode(true);
                                break;

                            case 4:     // Ps = 4  -> Smooth (Slow) Scroll (DECSCLM).
                                controller.EnableSmoothScrollMode(true);
                                break;

                            case 5:     // Ps = 5  -> Reverse Video (DECSCNM).
                                controller.EnableReverseVideoMode(true);
                                break;

                            case 6:     // Ps = 6  -> Origin Mode (DECOM).
                                controller.EnableOriginMode(true);
                                break;

                            case 7:     // Ps = 7  -> Wraparound Mode (DECAWM).
                                controller.EnableWrapAroundMode(true);
                                break;

                            case 8:     // Ps = 8  -> Auto-repeat Keys (DECARM).
                                controller.EnableAutoRepeatKeys(true);
                                break;

                            case 12:    // Ps = 1 2  -> Start Blinking Cursor (AT&T 610).
                                controller.EnableBlinkingCursor(true);
                                break;

                            case 25:    // Ps = 2 5  -> Show Cursor (DECTCEM).
                                controller.ShowCursor(true);
                                break;

                            case 40:    // Ps = 4 0  -> Allow 80 -> 132 Mode.
                                controller.Enable80132Mode(true);
                                break;

                            case 45:    // Ps = 4 5  -> Reverse-wraparound Mode.
                                controller.EnableReverseWrapAroundMode(true);
                                break;

                            case 47:    // Ps = 4 7  -> Use Alternate Screen Buffer.  (This may be disabled by the titeInhibit resource).
                                controller.EnableAlternateBuffer();
                                break;

                            case 69:    // Ps = 6 9  -> Enable left and right margin mode (DECLRMM), VT420 and up.
                                controller.EnableLeftAndRightMarginMode(true);
                                break;

                            case 1001:  // Ps = 1 0 0 1  -> Use Hilite Mouse Tracking.
                                controller.UseHighlightMouseTracking(true);
                                break;

                            case 1002:  // Ps = 1 0 0 2  -> Use Cell Motion Mouse Tracking.
                                controller.UseCellMotionMouseTracking(true);
                                break;

                            case 1006:  // Ps = 1 0 0 6  -> Enable SGR Mouse Mode.
                                controller.EnableSgrMouseMode(true);
                                break;

                            case 1049:  // Ps = 1 0 4 9  -> Save cursor as in DECSC and use Alternate Screen Buffer, clearing it first.
                                controller.SaveCursor();
                                controller.EnableAlternateBuffer();
                                break;

                            case 2004:  // Ps = 2 0 0 4  -> Set bracketed paste mode.
                                controller.SetBracketedPasteMode(true);
                                break;

                            default:
                                System.Diagnostics.Debug.WriteLine("Reset Mode (RM) mode: " + parameter.ToString() + " is unknown");
                                break;
                        }
                    }
                }
            },
            new SequenceHandler
            {
                Description = "Character Position Relative Backwards [columns] (default = [row,col-1])",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "j",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.MoveCursorRelative(-count, 0);
                }
            },
            new SequenceHandler
            {
                Description = "Vertical position backwards",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "k",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.MoveCursorRelative(0, -count);
                }
            },
            new SequenceHandler
            {
                Description = "Reset Mode (RM).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "l",
                Handler = (sequence, controller) =>
                {
                    foreach(var parameter in sequence.Parameters)
                    {
                        switch(parameter)
                        {
                            case 4:     // Ps = 4  -> Replace Mode (IRM).
                                controller.SetInsertReplaceMode(EInsertReplaceMode.Replace);
                                break;

                            case 20:    // Ps = 2 0  -> Normal Linefeed (LNM).
                                controller.SetAutomaticNewLine(false);
                                break;

                            default:
                                System.Diagnostics.Debug.WriteLine("Reset Mode (RM) mode: " + parameter.ToString() + " is unknown");
                                break;
                        }
                    }
                }
            },
            new SequenceHandler
            {
                Description = "DEC Private Mode Reset (DECRST).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "l",
                Query = true,
                Handler = (sequence, controller) =>
                {
                    foreach(var parameter in sequence.Parameters)
                    {
                        switch(parameter)
                        {
                            case 1:     // Ps = 1  -> Normal Cursor Keys (DECCKM).
                                controller.EnableApplicationCursorKeys(false);
                                break;

                            case 3:     // Ps = 3  -> 80 Column Mode (DECCOLM).
                                controller.Enable132ColumnMode(false);
                                break;

                            case 4:     // Ps = 4  -> Jump (Fast) Scroll (DECSCLM).
                                controller.EnableSmoothScrollMode(false);
                                break;

                            case 5:     // Ps = 5  -> Normal Video (DECSCNM).
                                controller.EnableReverseVideoMode(false);
                                break;

                            case 6:     // Ps = 6  -> Normal Cursor Mode (DECOM).
                                controller.EnableOriginMode(false);
                                break;

                            case 7:     // Ps = 7  -> No Wraparound Mode (DECAWM).
                                controller.EnableWrapAroundMode(false);
                                break;

                            case 8:     // Ps = 8  -> No Auto-repeat Keys (DECARM).
                                controller.EnableAutoRepeatKeys(false);
                                break;

                            case 12:    // Ps = 1 2  -> Stop Blinking Cursor (AT&T 610).
                                controller.EnableBlinkingCursor(false);
                                break;

                            case 25:    // Ps = 2 5  -> Hide Cursor (DECTCEM).
                                controller.ShowCursor(false);
                                break;

                            case 40:    // Ps = 4 0  -> Disallow 80 -> 132 Mode.
                                controller.Enable80132Mode(false);
                                break;

                            case 45:    // Ps = 4 5  -> No Reverse-wraparound Mode.
                                controller.EnableReverseWrapAroundMode(false);
                                break;

                            case 47:    // Ps = 4 7  -> Use Normal Screen Buffer.
                                controller.EnableNormalBuffer();
                                break;

                            case 69:    // Ps = 6 9  -> Disable left and right margin mode (DECLRMM), VT420 and up.
                                controller.EnableLeftAndRightMarginMode(false);
                                break;

                            case 1001:  // Ps = 1 0 0 1  -> Don't use Hilite Mouse Tracking.
                                controller.UseHighlightMouseTracking(false);
                                break;

                            case 1002:  // Ps = 1 0 0 2  -> Don't use Cell Motion Mouse Tracking.
                                controller.UseCellMotionMouseTracking(false);
                                break;

                            case 1006:  // Ps = 1 0 0 6  -> Disable SGR Mouse Mode.
                                controller.EnableSgrMouseMode(false);
                                break;

                            case 1049:  // Ps = 1 0 4 9  -> Use Normal Screen Buffer and restore cursor as in DECRC.
                                controller.RestoreCursor();
                                controller.EnableNormalBuffer();
                                break;

                            case 2004:  // Ps = 2 0 0 4  -> Reset bracketed paste mode.
                                controller.SetBracketedPasteMode(false);
                                break;

                            default:
                                System.Diagnostics.Debug.WriteLine("Reset Mode (RM) mode: " + parameter.ToString() + " is unknown");
                                break;
                        }
                    }
                }
            },
            new SequenceHandler
            {
                Description = "Character Attributes (SGR).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "m",
                Handler = (sequence, controller) =>
                {
                    var csiSequence = sequence as CsiSequence;
                    if(csiSequence.Parameters.Count == 0)
                        controller.SetCharacterAttribute(0);
                    else if(csiSequence.Parameters[0] == 38 || csiSequence.Parameters[0] == 48)
                    {
                        if(csiSequence.Parameters.Count == 6 && csiSequence.Parameters[1] == 2)
                        {
                            // XTerm iRGB
                            if(csiSequence.Parameters[0] == 38)
                                controller.SetRgbForegroundColor(csiSequence.Parameters[3], csiSequence.Parameters[4], csiSequence.Parameters[5]);
                            else
                                controller.SetRgbBackgroundColor(csiSequence.Parameters[3], csiSequence.Parameters[4], csiSequence.Parameters[5]);
                        }
                        else if(csiSequence.Parameters.Count == 5 && csiSequence.Parameters[1] == 2)
                        {
                            // Konsole RGB
                            if(csiSequence.Parameters[0] == 38)
                                controller.SetRgbForegroundColor(csiSequence.Parameters[2], csiSequence.Parameters[3], csiSequence.Parameters[4]);
                            else
                                controller.SetRgbBackgroundColor(csiSequence.Parameters[2], csiSequence.Parameters[3], csiSequence.Parameters[4]);
                        }
                        else if(csiSequence.Parameters.Count == 3 && csiSequence.Parameters[1] == 5)
                        {
                            if(csiSequence.Parameters[0] == 38)
                                controller.SetIso8613PaletteForeground(csiSequence.Parameters[2]);
                            else
                                controller.SetIso8613PaletteBackground(csiSequence.Parameters[2]);
                        }
                        else
                            System.Diagnostics.Debug.WriteLine("SGR " + csiSequence.Parameters[0].ToString() + " must be longer than 1 option");
                    }
                    else
                    {
                        foreach(var parameter in csiSequence.Parameters)
                            controller.SetCharacterAttribute(parameter);
                    }
                }
            },
            new SequenceHandler
            {
                Description = "Device Status Report (DSR). - Status Report.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "n",
                ExactParameterCount = 1,
                Param0 = new int [] { 5 },
                Handler = (sequence, controller) => controller.DeviceStatusReport()
            },
            new SequenceHandler
            {
                Description = "Device Status Report (DSR). - Report Cursor Position (CPR)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "n",
                ExactParameterCount = 1,
                Param0 = new int [] { 6 },
                Handler = (sequence, controller) => controller.ReportCursorPosition()
            },
            new SequenceHandler
            {
                Description = "Request DEC private mode (DECRQM).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                Query = true,
                CsiCommand = "$p",
                ExactParameterCount = 1,
                Handler = (sequence, controller) => controller.RequestDecPrivateMode(sequence.Parameters[0])
            },
            new SequenceHandler
            {
                Description = "Soft terminal reset (DECSTR).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "p",
                Bang = true,
                Handler = (sequence, controller) => controller.FullReset()
            },
            new SequenceHandler
            {
                Description = "Select character protection attribute (DECSCA).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "\"q",
                ExactParameterCountOrDefault = 1,
                Param0 = new int [] { 0, 2 },
                Handler = (sequence, controller) => controller.ProtectCharacter(false)
            },
            new SequenceHandler
            {
                Description = "Select character protection attribute (DECSCA).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "\"q",
                ExactParameterCount = 1,
                Param0 = new int [] { 1 },
                Handler = (sequence, controller) => controller.ProtectCharacter(true)
            },
            new SequenceHandler
            {
                Description = "Set cursor style (DECSCUSR, VT520).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = " q",
                ExactParameterCountOrDefault = 1,
                Param0 = new int [] { 0, 1, 2, 3, 4, 5, 6 },
                Handler = (sequence, controller) => {
                    var shape = ECursorShape.Block;
                    var blink = true;
                    if(sequence.Parameters.Count == 1)
                    {
                        switch(sequence.Parameters[0])
                        {
                            case 0:
                            case 1:
                                shape = ECursorShape.Block;
                                blink = true;
                                break;

                            case 2:
                                shape = ECursorShape.Block;
                                blink = false;
                                break;

                            case 3:
                                shape = ECursorShape.Underline;
                                blink = true;
                                break;

                            case 4:
                                shape = ECursorShape.Underline;
                                blink = false;
                                break;

                            case 5:
                                shape = ECursorShape.Bar;
                                blink = true;
                                break;

                            case 6:
                                shape = ECursorShape.Bar;
                                blink = false;
                                break;
                        }
                    }
                    controller.SetCursorStyle(shape, blink);
                }
            },
            new SequenceHandler
            {
                Description = "Set Scrolling Region [top;bottom] (default = full size of window)  (DECSTBM).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "r",
                ExactParameterCountOrDefault = 2,
                Handler = (sequence, controller) =>
                {
                    if(sequence.Parameters.Count == 0)
                        controller.ClearScrollingRegion();
                    else if(sequence.Parameters.Count == 2)
                        controller.SetScrollingRegion(sequence.Parameters[0], sequence.Parameters[1]);
                }
            },
            new SequenceHandler
            {
                Description = "Restore Cursor Keys",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "r",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 1 },
                Handler = (sequence, controller) => controller.RestoreCursorKeys()
            },
            new SequenceHandler
            {
                Description = "Restore Normal Screen Buffer.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "r",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 47 },
                Handler = (sequence, controller) => controller.RestoreEnableNormalBuffer()
            },
            new SequenceHandler
            {
                Description = "Restore Hilite Mouse Tracking.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "r",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 1001 },
                Handler = (sequence, controller) => controller.RestoreUseHighlightMouseTracking()
            },
            new SequenceHandler
            {
                Description = "Restore use Hilite Mouse Tracking.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "r",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 1002 },
                Handler = (sequence, controller) => controller.RestoreUseCellMotionMouseTracking()
            },
            new SequenceHandler
            {
                Description = "Restore SGR Mouse Mode.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "r",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 1006 },
                Handler = (sequence, controller) => controller.RestoreEnableSgrMouseMode()
            },
            new SequenceHandler
            {
                Description = "Restore bracketed paste mode.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "r",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 2004 },
                Handler = (sequence, controller) => controller.RestoreBracketedPasteMode()
            },
            new SequenceHandler
            {
                Description = "Set left and right margins (DECSLRM), available only when DECLRMM is enabled",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "s",
                ExactParameterCount = 2,
                Handler = (sequence, controller) =>
                {
                    if(sequence.Parameters.Count == 2)
                        controller.SetLeftAndRightMargins(sequence.Parameters[0], sequence.Parameters[1]);
                }
            },
            new SequenceHandler
            {
                Description = "Save Cursor Keys",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "s",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 1 },
                Handler = (sequence, controller) => controller.SaveCursorKeys()
            },
            new SequenceHandler
            {
                Description = "Save Normal Screen Buffer.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "s",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 47 },
                Handler = (sequence, controller) => controller.SaveEnableNormalBuffer()
            },
            new SequenceHandler
            {
                Description = "Save Hilite Mouse Tracking.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "s",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 1001 },
                Handler = (sequence, controller) => controller.SaveUseHighlightMouseTracking()
            },
            new SequenceHandler
            {
                Description = "Save use Hilite Mouse Tracking.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "s",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 1002 },
                Handler = (sequence, controller) => controller.SaveUseCellMotionMouseTracking()
            },
            new SequenceHandler
            {
                Description = "Save SGR Mouse Mode.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "s",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 1006 },
                Handler = (sequence, controller) => controller.SaveEnableSgrMouseMode()
            },
            new SequenceHandler
            {
                Description = "Save bracketed paste mode.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "s",
                Query = true,
                ExactParameterCount = 1,
                Param0 = new int [] { 2004 },
                Handler = (sequence, controller) => controller.SaveBracketedPasteMode()
            },
            new SequenceHandler
            {
                Description = "Cursor Up Ps Times (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "A",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) =>
                {
                    controller.MoveCursorRelative(0, -((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]));
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Down Ps Times (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "B",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) =>
                {
                    controller.MoveCursorRelative(0, ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]));
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Forward Ps Times (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "C",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) =>
                {
                    controller.MoveCursorRelative(((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]), 0);
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Backward Ps Times (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "D",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) =>
                {
                    controller.MoveCursorRelative(-((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]), 0);
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Next Line Ps Times (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "E",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.CarriageReturn();
                    controller.MoveCursorRelative(0, count);
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Preceding Line Ps Times (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "F",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.CarriageReturn();
                    while((count--) > 0)
                        controller.ReverseIndex();
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Character Absolute  [column] (default = [row,1])",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "G",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.CarriageReturn();
                    controller.MoveCursorRelative(count - 1, 0);
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Position [row;column] (default = [1,1])",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "H",
                ExactParameterCountOrDefault = 2,
                Handler = (sequence, controller) =>
                {
                    if(sequence.Parameters == null || sequence.Parameters.Count == 0)
                        controller.SetCursorPosition(1,1);
                    else
                        controller.SetCursorPosition(sequence.Parameters[1], sequence.Parameters[0]);
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Position [row;1]",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "H",
                ExactParameterCount = 1,
                Handler = (sequence, controller) => controller.SetCursorPosition(1, sequence.Parameters[0])
            },
            new SequenceHandler
            {
                Description = "Cursor Forward Tabulation Ps tab stops (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "I",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    while((count--) > 0)
                        controller.Tab();
                }
            },
            new SequenceHandler
            {
                Description = "Erase in Display - Selective Erase Below (default)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "J",
                ExactParameterCountOrDefault = 1,
                Param0 = new int [] { 0 },
                Handler = (sequence, controller) => controller.EraseBelow()
            },
            new SequenceHandler
            {
                Description = "Erase in Display - Selective Erase Above.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "J",
                ExactParameterCount = 1,
                Param0 = new int [] { 1 },
                Handler = (sequence, controller) => controller.EraseAbove()
            },
            new SequenceHandler
            {
                Description = "Erase in Display - Selective Erase All.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "J",
                ExactParameterCount = 1,
                Param0 = new int [] { 2 },
                Handler = (sequence, controller) => controller.EraseAll()
            },
            new SequenceHandler
            {
                Description = "Erase in Line - Selective Erase to Right (default)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "K",
                ExactParameterCountOrDefault = 1,
                Param0 = new int [] { 0 },
                Handler = (sequence, controller) => controller.EraseToEndOfLine()
            },
            new SequenceHandler
            {
                Description = "Erase in Line - Selective Erase to Left.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "K",
                ExactParameterCount = 1,
                Param0 = new int [] { 1 },
                Handler = (sequence, controller) => controller.EraseToStartOfLine()
            },
            new SequenceHandler
            {
                Description = "Erase in Line - Selective Erase All.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "K",
                ExactParameterCount = 1,
                Param0 = new int [] { 2 },
                Handler = (sequence, controller) => controller.EraseLine()
            },
            new SequenceHandler
            {
                Description = "Insert Ps Line(s) (default = 1) (IL).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "L",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => controller.InsertLines((sequence.Parameters == null || sequence.Parameters.Count == 0) ? 1 : sequence.Parameters[0])
            },
            new SequenceHandler
            {
                Description = "Delete Ps Line(s) (default = 1) (DL).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "M",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => controller.DeleteLines((sequence.Parameters == null || sequence.Parameters.Count == 0) ? 1 : sequence.Parameters[0])
            },
            new SequenceHandler
            {
                Description = "Delete Ps Character(s) (default = 1)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "P",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) =>
                {
                    controller.DeleteCharacter((sequence.Parameters == null || sequence.Parameters.Count == 0) ? 1 : sequence.Parameters[0]);
                }
            },
            new SequenceHandler
            {
                Description = "Scroll up Ps lines (default = 1) (SU)",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "S",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) =>
                {
                    controller.Scroll((sequence.Parameters == null || sequence.Parameters.Count == 0) ? 1 : sequence.Parameters[0]);
                }
            },
            new SequenceHandler
            {
                Description = "Scroll down Ps lines (default = 1) (SD).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "T",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) =>
                {
                    controller.Scroll(-((sequence.Parameters == null || sequence.Parameters.Count == 0) ? 1 : sequence.Parameters[0]));
                }
            },
            new SequenceHandler
            {
                Description = "CSI Ps X  Erase Ps Character(s) (default = 1) (ECH).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "X",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.EraseCharacter(count);
                }
            },
            new SequenceHandler
            {
                Description = "Cursor Backward Tabulation Ps tab stops (default = 1) (CBT).",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "Z",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    while((count--) > 0)
                        controller.ReverseTab();
                }
            },
            new SequenceHandler
            {
                Description = "Character Position Absolute  [column] (default = [row,1])",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "`",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.CarriageReturn();
                    controller.MoveCursorRelative(count - 1, 0);
                }
            },
            new SequenceHandler
            {
                Description = "Insert Ps Column(s) (default = 1) (DECIC), VT420 and up.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "'}",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.InsertColumn(count);
                }
            },
            new SequenceHandler
            {
                Description = "Delete Ps Column(s) (default = 1) (DECDC), VT420 and up.",
                SequenceType = SequenceHandler.ESequenceType.CSI,
                CsiCommand = "'~",
                ExactParameterCountOrDefault = 1,
                Handler = (sequence, controller) => {
                    var count = ((sequence.Parameters == null || sequence.Parameters.Count == 0 || sequence.Parameters[0] == 0) ? 1 : sequence.Parameters[0]);
                    controller.DeleteColumn(count);
                }
            },
            new SequenceHandler
            {
                Description = "Select default character set.  That is ISO 8859-1",
                SequenceType = SequenceHandler.ESequenceType.Unicode,
                CsiCommand = "@",
                Handler = (sequence, controller) => controller.SetLatin1()
            },
            new SequenceHandler
            {
                Description = "Select UTF-8 character set (ISO 2022).",
                SequenceType = SequenceHandler.ESequenceType.Unicode,
                CsiCommand = "G",
                Handler = (sequence, controller) => controller.SetUTF8()
            },
        };

        public static void ProcessSequence(TerminalSequence sequence, IVirtualTerminalController controller)
        {
            if(sequence is CharacterSequence)
            {
                var handler = Handlers.Where(x => x.SequenceType == SequenceHandler.ESequenceType.Character).SingleOrDefault();
                if (handler == null)
                    throw new Exception("There are no sequence handlers configured for type CharacterSequence");

                handler.Handler(sequence, controller);

                return;
            }

            if (sequence is CsiSequence)
            {
                var handler = Handlers
                    .Where(x =>
                        x.SequenceType == SequenceHandler.ESequenceType.CSI &&
                        x.CsiCommand == sequence.Command &&
                        x.Query == sequence.IsQuery &&
                        x.Send == sequence.IsSend &&
                        x.Bang == sequence.IsBang &&
                        (
                            (
                                x.Param0.Length == 0 && 
                                x.ValidParams.Length == 0
                            )
                            ||
                            (
                                x.Param0.Length > 0 &&
                                x.Param0.Contains(
                                    (sequence.Parameters == null || sequence.Parameters.Count < 1) ?
                                    0 :
                                    sequence.Parameters[0]
                                )
                            )
                            ||
                            (
                                x.ValidParams.Length > 0 &&
                                sequence.Parameters != null &&
                                !x.ValidParams.Except(sequence.Parameters).Any()
                            )
                        )
                        &&
                        (
                            (
                                x.ExactParameterCount == -1 &&
                                x.ExactParameterCountOrDefault == -1
                            ) ||
                            (
                                x.ExactParameterCount != -1 &&
                                (sequence.Parameters != null && x.ExactParameterCount == sequence.Parameters.Count)
                            ) ||
                            (
                                x.ExactParameterCountOrDefault != -1 &&
                                (
                                    sequence.Parameters == null ||
                                    sequence.Parameters.Count == 0 ||
                                    x.ExactParameterCountOrDefault == sequence.Parameters.Count
                                )
                            )
                        )
                    )
                    .SingleOrDefault();

                if (handler == null)
                    throw new Exception("There are no CsiSequence handlers configured for sequence: " + sequence.ToString());

                handler.Handler(sequence, controller);

                return;
            }

            if (sequence is OscSequence)
            {
                if (sequence.Parameters.Count < 1)
                    throw new Exception("OSC sequence doesn't have any parameters");

                var handler = Handlers.Where(x => x.SequenceType == SequenceHandler.ESequenceType.OSC && x.Param0.Contains(sequence.Parameters[0])).SingleOrDefault();
                if (handler == null)
                    throw new Exception("There are no sequence handlers configured for type OscSequence with param0 = " + sequence.Parameters[0].ToString());

                handler.Handler(sequence, controller);

                return;
            }

            if (sequence is CharacterSetSequence)
            {
                var handler = Handlers.Where(x => x.SequenceType == SequenceHandler.ESequenceType.CharacterSet).SingleOrDefault();
                if (handler == null)
                    throw new EscapeSequenceException("There are no sequence handlers configured for type CharacterSetSequence " + sequence.ToString(), sequence);

                handler.Handler(sequence, controller);

                return;
            }

            if (sequence is EscapeSequence)
            {
                var handler = Handlers
                    .Where(x => 
                        x.SequenceType == SequenceHandler.ESequenceType.Escape &&
                        x.CsiCommand == sequence.Command
                    )
                    .SingleOrDefault();

                if (handler == null)
                    throw new EscapeSequenceException("There are no sequence handlers configured for type EscapeSequence " + sequence.ToString(), sequence);

                handler.Handler(sequence, controller);

                return;
            }

            if (sequence is CharacterSizeSequence)
            {
                var handler = Handlers.Where(x => x.SequenceType == SequenceHandler.ESequenceType.CharacterSize).SingleOrDefault();
                if (handler == null)
                    throw new EscapeSequenceException("There are no sequence handlers configured for type CharacterSizeSequence " + sequence.ToString(), sequence);

                handler.Handler(sequence, controller);

                return;
            }

            if (sequence is UnicodeSequence)
            {
                var handler = Handlers
                    .Where(x =>
                        x.SequenceType == SequenceHandler.ESequenceType.Unicode &&
                        x.CsiCommand == sequence.Command
                    )
                    .SingleOrDefault();

                if (handler == null)
                    throw new EscapeSequenceException("There are no sequence handlers configured for type UnicodeSequence " + sequence.ToString(), sequence);

                handler.Handler(sequence, controller);

                return;
            }

            if (sequence is SS2Sequence)
            {
                controller.PutG2Char(sequence.Command[0]);
                return;
            }

            if (sequence is SS3Sequence)
            {
                controller.PutG3Char(sequence.Command[0]);
                return;
            }

            System.Diagnostics.Debug.WriteLine("Unhandled sequence -> " + sequence.ToString());
        }
    }
}
