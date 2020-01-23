#!/usr/bin/env csexec.sh
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class Program
{
	
	const string ConversationTextBodyRegex = @"((?<sayParams>[\w ""><.'-_]*?):)? *""(?<text>.*)""\r*(\n|$)";
	const string SayWithVoiceRegex = @"((?<sayParams>[\w ""><.'-_]*?):) *""(?<text>.*)"" ""(?<voice>.*)""\r*(\n|$)";
    	const string ChoiceRegex = @"(@choice )""([\w ]*)"" ?(?:if \((.+)\))?";
public static void Main(string[] args)
    {


	string lines = File.ReadAllText(args[0]);

	
	
	string luaCode = Parse(lines);


	Console.WriteLine(luaCode);
    }
	public static string Parse(string lines) {

		lines = Regex.Replace(lines,SayWithVoiceRegex, "say(\"${sayParams}:${text}\",\"${voice}\")");
		lines = Regex.Replace(lines, @"[ \t]*\$ (.*)", "$1");
		lines = Regex.Replace(lines, @"[ \t]*label (.*)", "::$1::");
		lines = Regex.Replace(lines, @"[ \t]*jump (.*)", "goto $1");
		lines = Regex.Replace(lines, @"[ \t]*(clearmenu)|(clear)", "clearmenu()");
		lines = Regex.Replace(lines, $"^([ \t]*{ConversationTextBodyRegex})+", "\nconversation [[\n$&\n]]\n", RegexOptions.Multiline);
		lines = Regex.Replace(lines, @"(#|;|(--)|(\/\/)).*", "");
		lines = Regex.Replace(lines, @"hide (\w*)", "stage.hide(\"$1\")");
		lines = Regex.Replace(lines, @"show ([\w ""]*)", "stage.show($1)");
		lines = lines.Replace("\" \"", "\", \"");
		return lines;
	}

	


	

}
