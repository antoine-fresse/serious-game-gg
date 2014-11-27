//----------------------------------------------
//    GoogleFu: Google Doc Unity integration
//         Copyright Â© 2013 Litteratus
//
//        This file has been auto-generated
//              Do not manually edit
//----------------------------------------------

using UnityEngine;

namespace GoogleFu
{
	[System.Serializable]
	public class ActorDBRow 
	{
		public string _NAME;
		public string _GROUP;
		public string _DESC;
		public int _CORRUPTIONCOST;
		public int _SEXISMECOST;
		public int _ATTACK;
		public int _REPUTATION;
		public string _CARDEFFECT;
		public string _CARDEFFECTDESC;
		public ActorDBRow(string __NAME, string __GROUP, string __DESC, string __CORRUPTIONCOST, string __SEXISMECOST, string __ATTACK, string __REPUTATION, string __CARDEFFECT, string __CARDEFFECTDESC) 
		{
			_NAME = __NAME.Trim();
			_GROUP = __GROUP.Trim();
			_DESC = __DESC.Trim();
			{
			int res;
				if(int.TryParse(__CORRUPTIONCOST, out res))
					_CORRUPTIONCOST = res;
				else
					Debug.LogError("Failed To Convert CORRUPTIONCOST string: "+ __CORRUPTIONCOST +" to int");
			}
			{
			int res;
				if(int.TryParse(__SEXISMECOST, out res))
					_SEXISMECOST = res;
				else
					Debug.LogError("Failed To Convert SEXISMECOST string: "+ __SEXISMECOST +" to int");
			}
			{
			int res;
				if(int.TryParse(__ATTACK, out res))
					_ATTACK = res;
				else
					Debug.LogError("Failed To Convert ATTACK string: "+ __ATTACK +" to int");
			}
			{
			int res;
				if(int.TryParse(__REPUTATION, out res))
					_REPUTATION = res;
				else
					Debug.LogError("Failed To Convert REPUTATION string: "+ __REPUTATION +" to int");
			}
			_CARDEFFECT = __CARDEFFECT.Trim();
			_CARDEFFECTDESC = __CARDEFFECTDESC.Trim();
		}

		public int Length { get { return 9; } }

		public string this[int i]
		{
		    get
		    {
		        return GetStringDataByIndex(i);
		    }
		}

		public string GetStringDataByIndex( int index )
		{
			string ret = System.String.Empty;
			switch( index )
			{
				case 0:
					ret = _NAME.ToString();
					break;
				case 1:
					ret = _GROUP.ToString();
					break;
				case 2:
					ret = _DESC.ToString();
					break;
				case 3:
					ret = _CORRUPTIONCOST.ToString();
					break;
				case 4:
					ret = _SEXISMECOST.ToString();
					break;
				case 5:
					ret = _ATTACK.ToString();
					break;
				case 6:
					ret = _REPUTATION.ToString();
					break;
				case 7:
					ret = _CARDEFFECT.ToString();
					break;
				case 8:
					ret = _CARDEFFECTDESC.ToString();
					break;
			}

			return ret;
		}

		public string GetStringData( string colID )
		{
			string ret = System.String.Empty;
			switch( colID.ToUpper() )
			{
				case "NAME":
					ret = _NAME.ToString();
					break;
				case "GROUP":
					ret = _GROUP.ToString();
					break;
				case "DESC":
					ret = _DESC.ToString();
					break;
				case "CORRUPTIONCOST":
					ret = _CORRUPTIONCOST.ToString();
					break;
				case "SEXISMECOST":
					ret = _SEXISMECOST.ToString();
					break;
				case "ATTACK":
					ret = _ATTACK.ToString();
					break;
				case "REPUTATION":
					ret = _REPUTATION.ToString();
					break;
				case "CARDEFFECT":
					ret = _CARDEFFECT.ToString();
					break;
				case "CARDEFFECTDESC":
					ret = _CARDEFFECTDESC.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "NAME" + " : " + _NAME.ToString() + "} ";
			ret += "{" + "GROUP" + " : " + _GROUP.ToString() + "} ";
			ret += "{" + "DESC" + " : " + _DESC.ToString() + "} ";
			ret += "{" + "CORRUPTIONCOST" + " : " + _CORRUPTIONCOST.ToString() + "} ";
			ret += "{" + "SEXISMECOST" + " : " + _SEXISMECOST.ToString() + "} ";
			ret += "{" + "ATTACK" + " : " + _ATTACK.ToString() + "} ";
			ret += "{" + "REPUTATION" + " : " + _REPUTATION.ToString() + "} ";
			ret += "{" + "CARDEFFECT" + " : " + _CARDEFFECT.ToString() + "} ";
			ret += "{" + "CARDEFFECTDESC" + " : " + _CARDEFFECTDESC.ToString() + "} ";
			return ret;
		}
	}
	public sealed class ActorDB
	{
		public enum rowIds {
			ACTOR_ADAMBALDWIN, ACTOR_ZOEQUINN, ACTOR_TFYC, ACTOR_DANIELVAVRA, ACTOR_MUNDANEMATT, ACTOR_TOTALBISCUIT, ACTOR_KOTAKU, ACTOR_GEORGESREESE, ACTOR_GAMASUTRA, ACTOR_POLYGON, ACTOR_ANITASARKEESIAN, ACTOR_INTERNETARISTOCRAT, ACTOR_DANIELLERIENDEAU
		};
		public string [] rowNames = {
			"ACTOR_ADAMBALDWIN", "ACTOR_ZOEQUINN", "ACTOR_TFYC", "ACTOR_DANIELVAVRA", "ACTOR_MUNDANEMATT", "ACTOR_TOTALBISCUIT", "ACTOR_KOTAKU", "ACTOR_GEORGESREESE", "ACTOR_GAMASUTRA", "ACTOR_POLYGON", "ACTOR_ANITASARKEESIAN", "ACTOR_INTERNETARISTOCRAT", "ACTOR_DANIELLERIENDEAU"
		};
		public System.Collections.Generic.List<ActorDBRow> Rows = new System.Collections.Generic.List<ActorDBRow>();

		public static ActorDB Instance
		{
			get { return NestedActorDB.instance; }
		}

		private class NestedActorDB
		{
			static NestedActorDB() { }
			internal static readonly ActorDB instance = new ActorDB();
		}

		private ActorDB()
		{
			Rows.Add( new ActorDBRow("Adam Baldwin",
														"GamerGate",
														"28 aout - adam baldwin lance le hashtag #GamerGate",
														"0",
														"0",
														"2",
														"6",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Zoe Quinn",
														"Anti-GamerGate",
														"Zoe Quinn",
														"0",
														"0",
														"6",
														"1",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("The Fine Young Capitalists",
														"None",
														"Groupe feministe qui a pour but d'aider les femmes dev",
														"0",
														"0",
														"1",
														"3",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Daniel Vavra",
														"GamerGate",
														"Designer de la saga \"Mafia\"",
														"0",
														"0",
														"2",
														"3",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("MundaneMatt",
														"GamerGate",
														"Youtuber",
														"0",
														"0",
														"2",
														"5",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("TotalBiscuit",
														"GamerGate",
														"Youtuber",
														"0",
														"0",
														"1",
														"4",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Kotaku",
														"News Website",
														"Gaming news website",
														"0",
														"0",
														"3",
														"2",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Georges Reese",
														"Anti-GamerGate",
														"Dell - a compar\u00e9 GamerGate \u00e0 ISIS ",
														"0",
														"0",
														"4",
														"5",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Gamasutra",
														"News Website",
														"Gaming news website",
														"0",
														"0",
														"2",
														"1",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Polygon",
														"News Website",
														"Gaming news website",
														"0",
														"0",
														"6",
														"6",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Anita Sarkeesian",
														"Anti-GamerGate",
														"Feministe",
														"0",
														"0",
														"1",
														"2",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Internet Aristocrat",
														"GamerGate",
														"Youtuber",
														"0",
														"0",
														"2",
														"3",
														"DefaultEffect",
														""));
			Rows.Add( new ActorDBRow("Danielle Riendeau",
														"Anti-GamerGate",
														"VG journalist - a donn\u00e9 une note de 10/10 au jeu d'un ami",
														"0",
														"0",
														"3",
														"4",
														"DefaultEffect",
														""));
		}
		public ActorDBRow GetRow(rowIds rowID)
		{
			ActorDBRow ret = null;
			try
			{
				ret = Rows[(int)rowID];
			}
			catch( System.Collections.Generic.KeyNotFoundException ex )
			{
				Debug.LogError( rowID + " not found: " + ex.Message );
			}
			return ret;
		}
		public ActorDBRow GetRow(string rowString)
		{
			ActorDBRow ret = null;
			try
			{
				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), rowString)];
			}
			catch(System.ArgumentException) {
				Debug.LogError( rowString + " is not a member of the rowIds enumeration.");
			}
			return ret;
		}

	}

}
