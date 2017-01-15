using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ConsoleApplication1
{
	class HowTo
	{
		private string name;
		private string url;
		
		public void setname(string s)
		{
			name=s;
		}
		public void seturl(string s)
		{
			url=s;
		}
		public string getname()
		{
			return name;
		}
		public string geturl()
		{
			return url;
		}
	}
    class Program
    {
        
        
        public static List<HowTo> how_to()
        {   List<HowTo> list=new List<HowTo>();
        	HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load("https://wiki.metakgp.org/w/List_of_how-tos");
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {  if(link.InnerText.Length>6){
            	if(link.InnerText.Substring(0,6).Equals("How to"))
            	{
            		HowTo temp=new HowTo();
            		temp.setname(link.InnerText);
            		temp.seturl(string.Concat("https://wiki.metakgp.org",link.GetAttributeValue("href",string.Empty)));
            		list.Add(temp);
            	}}
            }
            return list;
        }
        
        public static int similar(string s,string t)
        {
        	if (string.IsNullOrEmpty(s))
       		 {
            if (string.IsNullOrEmpty(t))
                return 0;
            return t.Length;
     		   }

        if (string.IsNullOrEmpty(t))
       	 {
            return s.Length;
       	 }

        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // initialize the top and right of the table to 0, 1, 2, ...
        for (int i = 0; i <= n; d[i, 0] = i++);
        for (int j = 1; j <= m; d[0, j] = j++);

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                int min1 = d[i - 1, j] + 1;
                int min2 = d[i, j - 1] + 1;
                int min3 = d[i - 1, j - 1] + cost;
                d[i, j] = Math.Min(Math.Min(min1, min2), min3);
            }
        }
        return d[n, m];
        }
        
        public static HowTo get_how_to(string s)
        {
        	List<HowTo> list=new List<HowTo>();
        	list=how_to();
        	int a=9999;
        	HowTo temp=new HowTo();
        	foreach(HowTo var in list)
        	{   int l=similar(var.getname(),s);
        		if(l<a)
        		   {   a=l;
        			temp=var;
        		}
        	}
        	return temp;
        }
        
        class eat_spots
        {   
        	public bool info=false;
        	public string name="";
        	public string category="";
        	public bool room_delivery=false;
        	public string url="";        	
        }
   
        static List<eat_spots> get_eat_spots()
        {   List<eat_spots> eat=new List<eat_spots>();
        	List<string> list =new List<string>(new String[]{"Amul parlour","Arabian Nights","Baskin Robbins","Billoos","Bimola Sweets","Carlos","Dreamland","Eggies","Flavour","Greenland","Heritage","Sahara","Subway","Super-Duper","Tikka"});
        	foreach(string var in list)
        	{
        		HtmlWeb hw = new HtmlWeb();
            	HtmlDocument doc = hw.Load("https://wiki.metakgp.org/w/Category:Food_and_Beverages");
            	foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            	{
            		if(link.InnerText.Equals(var))
            		{
            			eat_spots temp=new eat_spots();
            			string l=string.Concat("https://wiki.metakgp.org",link.GetAttributeValue("href",""));
            			HtmlWeb hw1 = new HtmlWeb();
           				HtmlDocument doc1 = hw1.Load(l);
           				foreach (HtmlNode table in doc1.DocumentNode.SelectNodes("//table"))
           				{
           					if(table.GetAttributeValue("class","").Equals("infobox vcard"))
           					{
           						foreach (HtmlNode row in table.SelectNodes("tr"))
           						{   List<string> type=new List<string>();
           							foreach (HtmlNode cell in row.SelectNodes("th|td"))
           							{
           								type.Add(cell.InnerText);
           							}
           							string a=type[1];
           							a=a.Substring(1,a.Length-1);
           							if(type[0]=="Category")
           								temp.category=a;
           							if(type[0]=="Room Delivery")
           							{
           								if(a.Equals("Yes"))
           									temp.room_delivery=true;
           							}
           							temp.url=l;
           							temp.info=true;
           						}
           					}
           				}
           				eat.Add(temp);
            			
            		}
            	}
        	}
        	return eat;
        }
        public static void Main(string[] args)
        {
        	/*string str=System.Console.ReadLine();
        	HowTo tem=new HowTo();
        	tem=get_how_to(str);
        	System.Console.WriteLine(tem.getname());
        	str=System.Console.ReadLine();*/
        	
        	/*HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load("https://wiki.metakgp.org/w/Amul_parlour");
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            { 
            	if(table.GetAttributeValue("class","").Equals("infobox vcard"))
            	{
            		foreach (HtmlNode row in table.SelectNodes("tr")) {
            			Console.WriteLine("row");
            			foreach (HtmlNode cell in row.SelectNodes("th|td")) {
           						 Console.WriteLine("cell: " + cell.InnerText);
       						 }
            		}
            	}
            	//string str=System.Console.ReadLine();
            	
            }*/
        	List<eat_spots> eat=new List<eat_spots>();
        	eat=get_eat_spots();
        }
        
    }
}
