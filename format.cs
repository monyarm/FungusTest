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
    	const string ChoiceRegex = @"(@choice )""([\w ]*)"" ?(?:if \((.+)\))?";
public static void Main(string[] args)
    {


	List<String> lines = File.ReadLines(args[0]).ToList();

	lines.ForEach(x => Console.WriteLine(x));

	List<String> luaCode = new List<String>();
	
	luaCode = Parse(lines);

	Console.WriteLine(" ");
	Console.WriteLine(" ");
	Console.WriteLine(" ");
	Console.WriteLine(" ");


	luaCode.ForEach(x => Console.WriteLine(x));
    }
	public static List<String> Parse(List<String> lines) {

	List<String> luaCode = new List<String>();
	var i = 0;
	for ( i = 0; i< lines.Count; i++){
		var x = lines[i];
                    		Console.WriteLine(lines[i].Trim());
		if (Regex.Match(x.Trim(), @"^\-\-\b").Success) {}
		
		else if (x.Trim() == "@menu") {

            		var menuLines = new List<String>();
			menuLines.Add(x.Trim());
			while (lines[i+1].Trim() != "@menuend")
                	{
                    		menuLines.Add(lines[i+1].Trim());
				i++;
                	}
			menuLines.Add(lines[i+1].Trim());
			i++;
			luaCode.AddRange(ParseMenu(menuLines));
		}
		else if (Regex.Match(x.Trim(), ConversationTextBodyRegex).Success) {
			luaCode.Add("conversation [[");

			luaCode.Add(x);

			while (Regex.Match(lines[i+1].Trim(), ConversationTextBodyRegex).Success){
				luaCode.Add(lines[i+1]);
				i++;		
			}

			luaCode.Add("]]");
			luaCode.Add("");			
		}
		else if (Regex.Match(x.Trim(), @"^\$\ \b").Success) {
			luaCode.Add(x.Trim().Trim('$').Trim());
			luaCode.Add("");	
		}
		

		}
		return luaCode;
	}

	public static List<String> ParseMenu(List<String> lines) {
		var menuLines = new List<String>();
		var i = 0;
		for ( i = 0; i< lines.Count; i++){
			var x = lines[i];
			var regMatch = Regex.Match(x, ChoiceRegex);
			if (regMatch.Success) {
				
				
				var choiceFunction = new List<String>();
				while (i+1 <lines.Count  && !lines[i+1].Trim().StartsWith("@choice")){
					Console.WriteLine(lines[i]);
					choiceFunction.Add(lines[i+1]);
					i++;		
				}
				string funcName = Regex.Replace(regMatch.Groups[2].Value, @"[^\w]", string.Empty);
				string funcString = "function "+funcName+" ()\n\n"+string.Join("\n", Parse(choiceFunction))+"\nend";
				menuLines.Add(funcString);
				menuLines.Add("");

				string menuFunction = ("menu("+regMatch.Groups[2].Value+"," +regMatch.Groups[2].Value+","+regMatch.Groups[3].Value +")").Replace(",)",")");
				menuLines.Add(menuFunction);
				menuLines.Add("");
			}
		}

		return menuLines;	
	}
	


	

}
