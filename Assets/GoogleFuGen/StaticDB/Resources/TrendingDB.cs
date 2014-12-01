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
	public class TrendingDBRow 
	{
		public string _NAME;
		public string _DESC;
		public float _CORRUPTIONMULT;
		public float _SEXISMEMULT;
		public string _CARDEFFECTDESC;
		public string _CARDEFFECT;
		public TrendingDBRow(string __NAME, string __DESC, string __CORRUPTIONMULT, string __SEXISMEMULT, string __CARDEFFECTDESC, string __CARDEFFECT) 
		{
			_NAME = __NAME.Trim();
			_DESC = __DESC.Trim();
			{
			float res;
				if(float.TryParse(__CORRUPTIONMULT, out res))
					_CORRUPTIONMULT = res;
				else
					Debug.LogError("Failed To Convert CORRUPTIONMULT string: "+ __CORRUPTIONMULT +" to float");
			}
			{
			float res;
				if(float.TryParse(__SEXISMEMULT, out res))
					_SEXISMEMULT = res;
				else
					Debug.LogError("Failed To Convert SEXISMEMULT string: "+ __SEXISMEMULT +" to float");
			}
			_CARDEFFECTDESC = __CARDEFFECTDESC.Trim();
			_CARDEFFECT = __CARDEFFECT.Trim();
		}

		public int Length { get { return 6; } }

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
					ret = _DESC.ToString();
					break;
				case 2:
					ret = _CORRUPTIONMULT.ToString();
					break;
				case 3:
					ret = _SEXISMEMULT.ToString();
					break;
				case 4:
					ret = _CARDEFFECTDESC.ToString();
					break;
				case 5:
					ret = _CARDEFFECT.ToString();
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
				case "DESC":
					ret = _DESC.ToString();
					break;
				case "CORRUPTIONMULT":
					ret = _CORRUPTIONMULT.ToString();
					break;
				case "SEXISMEMULT":
					ret = _SEXISMEMULT.ToString();
					break;
				case "CARDEFFECTDESC":
					ret = _CARDEFFECTDESC.ToString();
					break;
				case "CARDEFFECT":
					ret = _CARDEFFECT.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "NAME" + " : " + _NAME.ToString() + "} ";
			ret += "{" + "DESC" + " : " + _DESC.ToString() + "} ";
			ret += "{" + "CORRUPTIONMULT" + " : " + _CORRUPTIONMULT.ToString() + "} ";
			ret += "{" + "SEXISMEMULT" + " : " + _SEXISMEMULT.ToString() + "} ";
			ret += "{" + "CARDEFFECTDESC" + " : " + _CARDEFFECTDESC.ToString() + "} ";
			ret += "{" + "CARDEFFECT" + " : " + _CARDEFFECT.ToString() + "} ";
			return ret;
		}
	}
	public sealed class TrendingDB
	{
		public enum rowIds {
			TREND_GAMERGATE, TREND_NOTYOURSHIELD, TREND_DORITOS, TREND_STOPGAMERGATE, TREND_TROPESVSWOMEN
		};
		public string [] rowNames = {
			"TREND_GAMERGATE", "TREND_NOTYOURSHIELD", "TREND_DORITOS", "TREND_STOPGAMERGATE", "TREND_TROPESVSWOMEN"
		};
		public System.Collections.Generic.List<TrendingDBRow> Rows = new System.Collections.Generic.List<TrendingDBRow>();

		public static TrendingDB Instance
		{
			get { return NestedTrendingDB.instance; }
		}

		private class NestedTrendingDB
		{
			static NestedTrendingDB() { }
			internal static readonly TrendingDB instance = new TrendingDB();
		}

		private TrendingDB()
		{
			Rows.Add( new TrendingDBRow("#GamerGate is trending",
														"",
														"2",
														"0.5",
														"Double les malus de corruption et reduit les malus de sexisme",
														"DefaultEffect"));
			Rows.Add( new TrendingDBRow("#NotYourShield is trending",
														"Hashtag utilis\u00e9 par les minorit\u00e9s parmis les gamers",
														"2",
														"1",
														"Double les malus de corruption",
														"DefaultEffect"));
			Rows.Add( new TrendingDBRow("#Doritos is trending",
														"",
														"1",
														"1",
														"Aucun effet",
														"DefaultEffect"));
			Rows.Add( new TrendingDBRow("#StopGamerGate is trending",
														"",
														"0.5",
														"2",
														"Double les malus de sexisme et reduit les malus de corruption",
														"DefaultEffect"));
			Rows.Add( new TrendingDBRow("#TropesVsWomen is trending",
														"",
														"1",
														"2",
														"Double les malus de sexisme",
														"DefaultEffect"));
		}
		public TrendingDBRow GetRow(rowIds rowID)
		{
			TrendingDBRow ret = null;
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
		public TrendingDBRow GetRow(string rowString)
		{
			TrendingDBRow ret = null;
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
