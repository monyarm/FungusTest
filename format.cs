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
	

	static List<String> conversationCode = new List<String>();

	static List<String> menuCode = new List<String>();
    public static void Main(string[] args)
    {
	const string ConversationTextBodyRegex = @"((?<sayParams>[\w ""><.'-_]*?):)? *""(?<text>.*)""\r*(\n|$)";

	List<String> lines = File.ReadLines(args[0]).ToList();

	lines.ForEach(x => Console.WriteLine(x));

	List<String> luaCode = new List<String>();

	lines.ForEach(x => {
			

		
		if (Regex.Match(x.Trim(), @"^\-\-\b").Success) {}
		else if (Regex.Match(x.Trim(), @"^\$\ \b").Success) {
			luaCode.AddRange(ProcessConversation());
			luaCode.Add(x.Trim().Trim('$').Trim());	
		}
		else if (Regex.Match(x.Trim(), @"^menu\:\b").Success || Regex.Match(x.Trim(), @"^""[\w $]*""").Success) {
			luaCode.AddRange(ProcessConversation());
			menuCode.Add(x);
		}
		else if (Regex.Match(x.Trim(), ConversationTextBodyRegex).Success) {
			luaCode.AddRange(ProcessMenu());
			conversationCode.Add(x);			
		}
		else {
			luaCode.AddRange(ProcessConversation());
			luaCode.AddRange(ProcessMenu());
		}
		

	});


	Console.WriteLine(" ");
	Console.WriteLine(" ");
	Console.WriteLine(" ");
	Console.WriteLine(" ");


	luaCode.ForEach(x => Console.WriteLine(x));
    }

	public static List<String> ProcessMenu() {
		List<String> tempArr = new List<String>();
		if (menuCode.Count != 0) {
		
			//TEMP
			tempArr.Add("//Temp Menu");
			tempArr.AddRange(menuCode);
			tempArr.Add("//Temp Menu");
		}
		return tempArr;
	}

	public static List<String> ProcessConversation() {
		List<String> tempArr = new List<String>();
		if (conversationCode.Count != 0) {
			tempArr.Add("conversation [[");
			tempArr.AddRange(conversationCode);
			tempArr.Add("]]");
		}
		conversationCode.Clear();
		return tempArr;
	}


	

}
