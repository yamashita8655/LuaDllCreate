using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using LuaDLLTest;

public class luatest : MonoBehaviour {

	// Use this for initialization
	void Start () {

		test1 ();
//		test2 ();
//		test3 ();
//		test4 ();
//		test5 ();
//		StartCoroutine("LoadLuaFile");
//		bufferTest ();
	}

	// Androidでファイルを直接指定する場合で、StreamingAssetsにファイルを格納した場合は
	// こういう風に実体化させる
	// けど、Resources.LoadでTextAssetsにした方が簡単だと思うよ
	IEnumerator LoadLuaFile(){
		string path = Application.streamingAssetsPath + "/" + "load_lua.lua";
		WWW www = new WWW(path);
		yield return www;
		Debug.Log (www.text);
		Debug.Log ("file_exist:" + System.IO.File.Exists(Application.persistentDataPath + "/" + "load_lua.lua"));
		System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + "load_lua.lua", www.bytes);
		Debug.Log ("file_exist:" + System.IO.File.Exists(Application.persistentDataPath + "/" + "load_lua.lua"));

		path = Application.streamingAssetsPath + "/" + "function_lua.lua";
		www = new WWW(path);
		yield return www;
		Debug.Log (www.text);
		Debug.Log ("file_exist:" + System.IO.File.Exists(Application.persistentDataPath + "/" + "function_lua.lua"));
		System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + "function_lua.lua", www.bytes);
		Debug.Log ("file_exist:" + System.IO.File.Exists(Application.persistentDataPath + "/" + "function_lua.lua"));

		path = Application.streamingAssetsPath + "/" + "coroutine.lua";
		www = new WWW(path);
		yield return www;
		Debug.Log (www.text);
		Debug.Log ("file_exist:" + System.IO.File.Exists(Application.persistentDataPath + "/" + "coroutine.lua"));
		System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + "coroutine.lua", www.bytes);
		Debug.Log ("file_exist:" + System.IO.File.Exists(Application.persistentDataPath + "/" + "coroutine.lua"));

		path = Application.streamingAssetsPath + "/" + "UnityFunction.lua";
		www = new WWW(path);
		yield return www;
		Debug.Log (www.text);
		Debug.Log ("file_exist:" + System.IO.File.Exists(Application.persistentDataPath + "/" + "UnityFunction.lua"));
		System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + "UnityFunction.lua", www.bytes);
		Debug.Log ("file_exist:" + System.IO.File.Exists(Application.persistentDataPath + "/" + "UnityFunction.lua"));

		test1 ();
		test2 ();
		test3 ();
		test4 ();

	}

	void test1()
	{
		// こっちは、Resourcesフォルダに入っている物を直接参照して使うタイプ
		/*LuaState mLuaState = new LuaState();
		var ret = mLuaState.DoFile("load_lua");
		LuaLib.LuaGetGlobal (mLuaState.L, "windowWidth");
		LuaLib.LuaGetGlobal (mLuaState.L, "windowHeight");
		LuaLib.LuaGetGlobal (mLuaState.L, "windowName");
		LuaLib.LuaGetGlobal (mLuaState.L, "testboolean");

		printStack (mLuaState.L);*/

		// こっちは、何らかの方法でLuaスクリプトをバッファに展開して使うタイプ
		// これが出来たので、アセットバンドルに含めることも可能だと思う
		IntPtr mLuaState = NativeMethods.luaL_newstate();
		NativeMethods.luaL_openlibs(mLuaState);
		TextAsset file = Resources.Load<TextAsset>("load_lua");
		//int res = NativeMethods.LuaLLoadBuffer (mLuaState, file.text, (uint)file.bytes.Length, "load_lua");
		int res = NativeMethods.luaL_loadstring (mLuaState, file.text);
		res = NativeMethods.lua_pcallk (mLuaState, 0, -1, 0);
		NativeMethods.lua_getglobal(mLuaState, "windowWidth");
		NativeMethods.lua_getglobal(mLuaState, "windowHeight");
		NativeMethods.lua_getglobal(mLuaState, "windowName");
		NativeMethods.lua_getglobal(mLuaState, "testboolean");
		printStack (mLuaState);
	}

	void test2()
	{
/*		LuaState mLuaState = new LuaState();
		var ret = mLuaState.DoFile("function_lua");

		// Luaで定義した関数をスタックに積む。Luaは関数も変数のひとつに過ぎないらしい
		LuaLib.LuaGetGlobal(mLuaState.L, "calc");
		// 関数に指定する引数をスタックに積む
		LuaLib.LuaPushNumber(mLuaState.L, 100);
		LuaLib.LuaPushNumber(mLuaState.L, 200);
		
		int res = LuaLib.LuaPCall (mLuaState.L, 2, 4, 0);
		
		// 戻り値がスタックに積まれているらしいので、取得
		double add_res = LuaLib.LuaToNumber(mLuaState.L, 1);
		double sub_res = LuaLib.LuaToNumber(mLuaState.L, 2);
		double mult_res = LuaLib.LuaToNumber(mLuaState.L, 3);
		double dev_res = LuaLib.LuaToNumber(mLuaState.L, 4);
		
		printStack(mLuaState.L);*/
	}

	// コルーチンテスト
	//LuaState cotest_State;
	//LuaState co;
	void test3()
	{
/*		cotest_State = Lua.LuaOpen ();
		Lua.LuaOpenBase(cotest_State);
		int res = Lua.LuaLLoadFile (cotest_State, Application.persistentDataPath + "/" + "coroutine.lua");
		Lua.LuaPCall(cotest_State, 0, Lua.LUA_MULTRET, 0);

		co = Lua.LuaNewThread(cotest_State);
		Lua.LuaGetGlobal(co, "step");

//		printStack(co);

//		res = Lua.LuaResume (co, 0);

		//Lua.LuaClose (cotest_State);
		//printStack(cotest_State);*/
	}

	// 逆呼び出しテスト
	void test4()
	{
/*		LuaState lstate = Lua.LuaOpen ();
		Lua.LuaRegister (lstate, "UnityFunction", UnityFunction);
		int res = Lua.LuaLLoadFile (lstate, Application.persistentDataPath + "/" + "UnityFunction.lua");
		Lua.LuaPCall(lstate, 0, Lua.LUA_MULTRET, 0);*/
	}

	
	void printStack(System.IntPtr L)
	{
		int num = NativeMethods.lua_gettop (L);
		Debug.Log ("count = " + num);
		if(num==0)
		{
			return;
		}
		
		for(int i = num; i >= 1; i--)
		{
			int type = NativeMethods.lua_type(L, i);

			switch(type) {
			case 0://LuaTypes.LUA_TNIL:
				break;
			case 1://LuaTypes.LUA_TBOOLEAN:
				int res_b = NativeMethods.lua_toboolean(L, i);
				Debug.Log ("LUA_TBOOLEAN : " + res_b);
				break;
			case 2://LuaTypes.LUA_TLIGHTUSERDATA:
				break;
			case 3://LuaTypes.LUA_TNUMBER:
				double res_d = NativeMethods.lua_tonumberx(L, i, 0);
				Debug.Log ("LUA_TNUMBER : " + res_d);
				break;
			case 4://LuaTypes.LUA_TSTRING:
				uint res;
				IntPtr res_s = NativeMethods.lua_tolstring(L, i, out res);
				string resString = Marshal.PtrToStringAnsi(res_s);
				Debug.Log ("LUA_TSTRING : " + resString);
				break;
			case 5://LuaTypes.LUA_TTABLE:
				break;
			case 6://LuaTypes.LUA_TFUNCTION:
				break;
			case 7://LuaTypes.LUA_TUSERDATA:
				break;
			//case LuaTypes.LUA_TTHREAD:
			//	break;
			}
		}
	}

/*	void printStackKeralua(System.IntPtr L)
	{
		int num = KeraLua.Lua.LuaGetTop (L);
		if(num==0)
		{
			return;
		}

		public enum LuaTypes 
		{
			LUA_TNONE=-1,
			LUA_TNUMBER=3,
			LUA_TSTRING=4,
			LUA_TBOOLEAN=1,
			LUA_TTABLE=5,
			LUA_TFUNCTION=6,
			LUA_TUSERDATA=7,
			LUA_TLIGHTUSERDATA=2
		}
		
		for(int i = num; i >= 1; i--)
		{
			int type = KeraLua.Lua.LuaType(L, i);
			
			switch(type) {
			case 0:
				break;
			case 1:
				int res_b = KeraLua.Lua.LuaToBoolean(L, i);
				Debug.Log ("LUA_TBOOLEAN : " + res_b);
				break;
			case 2:
				break;
			case 3:
				double res_d = KeraLua.Lua.LuaNetToNumber(L, i);
				Debug.Log ("LUA_TNUMBER : " + res_d);
				break;
			case 4:
				uint res = 0;
				CharPtr res_s = KeraLua.Lua.LuaToLString(L, i, out res);
				Debug.Log ("LUA_TSTRING : " + res_s);
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
//			case 8:
//				break;
			}
		}
	}*/
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("click");


			if(cotest_State != null)
			{
				if(Lua.LuaResume(co, 0) != 0)
				{
					printStackKeralua(co);
				}
				else
				{
					Lua.LuaClose (cotest_State);
					cotest_State = null;
				}
			}
		}*/
	}

	void test5()
	{
		//LuaState lstate = Lua.LuaOpen ();
		//// 関数に指定する引数をスタックに積む
		//Chara data = new Chara ();
		//data.mNowHp = 100;
		//data.mMaxHp = 100;
		//data.mNowMp = 100;
		//data.mMaxMp = 100;
		//data.mAttackPoint = 10;
		//data.mDefencePoint = 5;

		//object obj = data;

		//int res = Lua.LuaLLoadFile (lstate, "C:/takuya/unity/luatest/Assets/luafunction/itemeffect.lua");
		//Lua.LuaPCall(lstate, 0, Lua.LUA_MULTRET, 0);

		//// Luaで定義した関数をスタックに積む。Luaは関数も変数のひとつに過ぎないらしい
		//Lua.LuaGetGlobal(lstate, "battle");

		//LuaTag tag = new LuaTag ();
		//tag.Tag = 1;
		//// 関数に指定する引数をスタックに積む
		//Lua.LuaPushLightUserData(lstate, tag);



		//// Lua関数を実行
		////if(Lua.LuaPCall(lstate, 2, 4, 0))
		////{
		////}

		//printStack(lstate);
		//res = Lua.LuaPCall (lstate, 1, 6, 0);

		//// 戻り値がスタックに積まれているらしいので、取得
		///*int nowhp = Lua.LuaToInteger(lstate, 1);
		//int maxhp = Lua.LuaToInteger(lstate, 2);
		//int nowmp = Lua.LuaToInteger(lstate, 3);
		//int maxmp = Lua.LuaToInteger(lstate, 4);
		//int atk = Lua.LuaToInteger(lstate, 5);
		//int def = Lua.LuaToInteger(lstate, 6);

		//printStack(lstate);*/

		//Lua.LuaClose (lstate);
	}

	int UnityFunction(System.IntPtr L)
	{
		Debug.Log ("UnityFunction");
		return 0;
	}

}
