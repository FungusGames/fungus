﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

using Fungus;

[TestFixture]
public class TextTagParserTests
{
	[Test]
	public void TextTagParser_Parser()
	{
		var textTagParser = new TextTagParser();

		// Parse an example string, generate correct sequence of tags
		List<TextTagParser.Token> tokens = textTagParser.Tokenize("Words " + 
		                                                          "{b}bold test{/b}" +
		                                                          "{i}italic test{/i}" +
		                                                          "{color=red}color test{/color}" +
		                                                          "{w}{w=0.5}" +
		                                                          "{wi}{wc}" +
		                                                          "{wp}{wp=0.5}{/wp}" +
		                                                          "{c}" +
		                                                          "{s}{s=60}{/s}" +
		                                                          "{x}{m=Message}" +
		                                                          "{vpunch=10,0.5}" +
		                                                          "{hpunch=10,0.5}" +
		                                                          "{punch=10,0.5}" +
		                                                          "{flash=0.5}" +
		                                                          "{audio=Sound}" +
		                                                          "{audioloop=Sound}" +
		                                                          "{audiopause=Sound}" +
		                                                          "{audiostop=Sound}"+ 
																  "{vpunch}" +
																  "{vpunch=10,0.5,0}" + 
																  "{hpunch}" +
																  "{hpunch=10,0.5,0}" + 
																  "{punch}" + 
																  "{punch=10,0.5,0}");

		int i = 0;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Words);
		Assert.That(tokens[i].paramList[0] == "Words ");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.BoldStart);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Words);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "bold test");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.BoldEnd);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.ItalicStart);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Words);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "italic test");
		
		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.ItalicEnd);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.ColorStart);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "red");
		
		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Words);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "color test");
		
		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.ColorEnd);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Wait);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Wait);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "0.5");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.WaitForInputNoClear);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.WaitForInputAndClear);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.WaitOnPunctuationStart);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.WaitOnPunctuationStart);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "0.5");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.WaitOnPunctuationEnd);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Clear);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.SpeedStart);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.SpeedStart);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "60");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.SpeedEnd);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Exit);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Message);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "Message");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.VerticalPunch);
		Assert.That(tokens[i].paramList.Count == 2);
		Assert.That(tokens[i].paramList[0] == "10");
		Assert.That(tokens[i].paramList[1] == "0.5");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.HorizontalPunch);
		Assert.That(tokens[i].paramList.Count == 2);
		Assert.That(tokens[i].paramList[0] == "10");
		Assert.That(tokens[i].paramList[1] == "0.5");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Punch);
		Assert.That(tokens[i].paramList.Count == 2);
		Assert.That(tokens[i].paramList[0] == "10");
		Assert.That(tokens[i].paramList[1] == "0.5");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Flash);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "0.5");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Audio);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "Sound");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.AudioLoop);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "Sound");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.AudioPause);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "Sound");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.AudioStop);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "Sound");

		//Few tests for incorrect numbers of parameters
		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.VerticalPunch);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.VerticalPunch);
		Assert.That(tokens[i].paramList.Count == 3);
		Assert.That(tokens[i].paramList[0] == "10");
		Assert.That(tokens[i].paramList[1] == "0.5");
		Assert.That(tokens[i].paramList[2] == "0");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.HorizontalPunch);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.HorizontalPunch);
		Assert.That(tokens[i].paramList.Count == 3);
		Assert.That(tokens[i].paramList[0] == "10");
		Assert.That(tokens[i].paramList[1] == "0.5");
		Assert.That(tokens[i].paramList[2] == "0");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Punch);
		Assert.That(tokens[i].paramList.Count == 0);

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Punch);
		Assert.That(tokens[i].paramList.Count == 3);
		Assert.That(tokens[i].paramList[0] == "10");
		Assert.That(tokens[i].paramList[1] == "0.5");
		Assert.That(tokens[i].paramList[2] == "0");

		Assert.That(tokens.Count == 37);
	}

	[Test]
	public void TextTagParser_AudioWaitBug()
	{
		var textTagParser = new TextTagParser();
		
		// Parse an example string, generate correct sequence of tags
		List<TextTagParser.Token> tokens = textTagParser.Tokenize("Play sound{audio=BeepSound}{w=1} Play loop{audioloop=BeepSound}{w=3} Stop{audiostop=BeepSound}");
		
		int i = 0;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Words);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "Play sound");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Audio);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "BeepSound");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Wait);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "1");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Words);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == " Play loop");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.AudioLoop);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "BeepSound");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Wait);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "3");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.Words);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == " Stop");

		i++;
		Assert.That(tokens[i].type == TextTagParser.TokenType.AudioStop);
		Assert.That(tokens[i].paramList.Count == 1);
		Assert.That(tokens[i].paramList[0] == "BeepSound");

		Assert.That(tokens.Count == 8);
	}
	
}
