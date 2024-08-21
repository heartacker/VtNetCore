using System;
using System.Collections.Generic;
using System.Text;
using VtNetCore.VirtualTerminal;
using VtNetCore.XTermParser;
using Xunit;

namespace VtNetCoreUnitTests
{
    public class BackgroundColorBug
    {
        private void Push(DataConsumer d, string s)
        {
            d.Push(Encoding.UTF8.GetBytes(s));
        }

        private bool IsCursor(VirtualTerminalController t, int row, int column)
        {
            return t.ViewPort.CursorPosition.Row == row && t.ViewPort.CursorPosition.Column == column;
        }

        private void FirstPage(VirtualTerminalController t, DataConsumer d)
        {
            Push(d, "\u001b[2J");
            Push(d, "\u001b[0;33;44m");
            Push(d, "\u001b[1;1H");
            Push(d, "\u001b[?7h");
            Push(d, "****************************************************************************************************************************************************************");
            Push(d, "\u001b[?7l");
            Push(d, "\u001b[3;1H");
            Push(d, "****************************************************************************************************************************************************************");
            Push(d, "\u001b[?7h");
            Push(d, "\u001b[5;1H");
            Push(d, "This should be three identical lines of *'s completely filling\r");
        }

        public static readonly string ExpectedFirstPage =
            "<foreground value='#C19C00' /><background value='#0037DA' />********************************************************************************↵" +
            "********************************************************************************↵" +
            "********************************************************************************↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "This should be three identical lines of *'s completely filling→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→";

         [Fact]
        public void TestFirstPage()
        {
            string s;
            var t = new VirtualTerminalController();
            var d = new DataConsumer(t);
            t.ResizeView(80, 24);
            //t.TestPatternScrollingDiagonalLower();

            FirstPage(t, d);

            s = t.PageAsSpans;

            Assert.Equal(ExpectedFirstPage, s);
        }

        public static readonly string ExpectedDarkBackgroundPage =
            "<foreground value='#C19C00' /><background value='#0037DA' />                   Graphic rendition test pattern:→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "<foreground value='#CCCCCC' /><background value='#0C0C0C' />vanilla<foreground value='#C19C00' /><background value='#0037DA' />                                <bright><foreground value='#FFFFFF' /><background value='#0C0C0C' />bold→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "</bright><foreground value='#C19C00' /><background value='#0037DA' />     <underscore><foreground value='#CCCCCC' /><background value='#0C0C0C' />underline</underscore><foreground value='#C19C00' /><background value='#0037DA' />                              <bright><underscore><foreground value='#FFFFFF' /><background value='#0C0C0C' />bold underline→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "<blink></bright></underscore><foreground value='#CCCCCC' />blink</blink><foreground value='#C19C00' /><background value='#0037DA' />                                  <blink><bright><foreground value='#FFFFFF' /><background value='#0C0C0C' />bold blink→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "</blink></bright><foreground value='#C19C00' /><background value='#0037DA' />     <blink><underscore><foreground value='#CCCCCC' /><background value='#0C0C0C' />underline blink</blink></underscore><foreground value='#C19C00' /><background value='#0037DA' />                        <blink><bright><underscore><foreground value='#FFFFFF' /><background value='#0C0C0C' />bold underline blink→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "</blink></bright><reverse></underscore><foreground value='#CCCCCC' />negative</reverse><foreground value='#C19C00' /><background value='#0037DA' />                               <bright><reverse><foreground value='#FFFFFF' /><background value='#0C0C0C' />bold negative→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "</bright></reverse><foreground value='#C19C00' /><background value='#0037DA' />     <reverse><underscore><foreground value='#CCCCCC' /><background value='#0C0C0C' />underline negative</reverse></underscore><foreground value='#C19C00' /><background value='#0037DA' />                     <bright><reverse><underscore><foreground value='#FFFFFF' /><background value='#0C0C0C' />bold underline negative→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "<blink></bright></underscore><foreground value='#CCCCCC' />blink negative</blink></reverse><foreground value='#C19C00' /><background value='#0037DA' />                         <blink><bright><reverse><foreground value='#FFFFFF' /><background value='#0C0C0C' />bold blink negative→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "</blink></bright></reverse><foreground value='#C19C00' /><background value='#0037DA' />     <blink><reverse><underscore><foreground value='#CCCCCC' /><background value='#0C0C0C' />underline blink negative</blink></reverse></underscore><foreground value='#C19C00' /><background value='#0037DA' />               <blink><bright><reverse><underscore><foreground value='#FFFFFF' /><background value='#0C0C0C' />bold underline blink negative→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→↵" +
            "</blink></bright></reverse></underscore><foreground value='#CCCCCC' />Dark background.Push<RETURN>                                                    ↵" +
            "→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→→";

        private void DarkBackgroundPage(VirtualTerminalController t, DataConsumer d)
        {
            Push(d, "\u001b[1;24r");
            Push(d, "\u001b[2J");
            Push(d, "\u001b[1;20H");
            Push(d, "Graphic rendition test pattern:");
            Push(d, "\u001b[4;1H");
            Push(d, "\u001b[0m");
            Push(d, "vanilla");
            Push(d, "\u001b[4;40H");
            Push(d, "\u001b[0;1m");
            Push(d, "bold");
            Push(d, "\u001b[6;6H");
            Push(d, "\u001b[;4m");
            Push(d, "underline");
            Push(d, "\u001b[6;45H");
            Push(d, "\u001b[;1m");
            Push(d, "\u001b[4m");
            Push(d, "bold underline");
            Push(d, "\u001b[8;1H");
            Push(d, "\u001b[0;5m");
            Push(d, "blink");
            Push(d, "\u001b[8;40H");
            Push(d, "\u001b[0;5;1m");
            Push(d, "bold blink");
            Push(d, "\u001b[10;6H");
            Push(d, "\u001b[0;4;5m");
            Push(d, "underline blink");
            Push(d, "\u001b[10;45H");
            Push(d, "\u001b[0;1;4;5m");
            Push(d, "bold underline blink");
            Push(d, "\u001b[12;1H");
            Push(d, "\u001b[1;4;5;0;7m");
            Push(d, "negative");
            Push(d, "\u001b[12;40H");
            Push(d, "\u001b[0;1;7m");
            Push(d, "bold negative");
            Push(d, "\u001b[14;6H");
            Push(d, "\u001b[0;4;7m");
            Push(d, "underline negative");
            Push(d, "\u001b[14;45H");
            Push(d, "\u001b[0;1;4;7m");
            Push(d, "bold underline negative");
            Push(d, "\u001b[16;1H");
            Push(d, "\u001b[1;4;;5;7m");
            Push(d, "blink negative");
            Push(d, "\u001b[16;40H");
            Push(d, "\u001b[0;1;5;7m");
            Push(d, "bold blink negative");
            Push(d, "\u001b[18;6H");
            Push(d, "\u001b[0;4;5;7m");
            Push(d, "underline blink negative");
            Push(d, "\u001b[18;45H");
            Push(d, "\u001b[0;1;4;5;7m");
            Push(d, "bold underline blink negative");
            Push(d, "\u001b[m");
            Push(d, "\u001b[?5l");
            Push(d, "\u001b[23;1H");
            Push(d, "\u001b[0K");
            Push(d, "Dark background.Push<RETURN>");
        }

        [Fact]
        public void TestDarkBackgroundPage()
        {
            string s;
            var t = new VirtualTerminalController();
            var d = new DataConsumer(t);
            t.ResizeView(80, 24);
            //t.TestPatternScrollingDiagonalLower();

            FirstPage(t, d);
            DarkBackgroundPage(t, d);

            s = t.PageAsSpans;

            Assert.Equal(ExpectedDarkBackgroundPage, s);
        }
    }
}
