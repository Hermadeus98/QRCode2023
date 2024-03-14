namespace QRCode.Engine.Toolbox.Pattern.StateMachine.Tests
{
	using Sirenix.OdinInspector;
	using UnityEngine;

	public class StateMachineExample : MonoBehaviour
	{
		private FiniteStateMachine _fsm = null;
		
		private void Start()
		{
			// 1. States creation
			State boot = new StateExample((int)GameFlowExample.Boot, GameFlowExample.Boot.ToString());
			State splashScreen = new StateExample((int)GameFlowExample.SplashScreen, GameFlowExample.SplashScreen.ToString());
			State mainMenu = new StateExample((int)GameFlowExample.MainMenu, GameFlowExample.MainMenu.ToString());
			State settingsMenu = new StateExample((int)GameFlowExample.SettingsMenu, GameFlowExample.SettingsMenu.ToString());
			State openWorld = new StateExample((int)GameFlowExample.OpenWorld, GameFlowExample.OpenWorld.ToString());
			State pause = new StateExample((int)GameFlowExample.Pause, GameFlowExample.Pause.ToString());
			State quitting = new StateExample((int)GameFlowExample.Quitting, GameFlowExample.Quitting.ToString());
			
			// 2. StateMachine creation
			_fsm = new FiniteStateMachine();
			_fsm.AddState(boot);
			_fsm.AddState(splashScreen);
			_fsm.AddState(mainMenu);
			_fsm.AddState(settingsMenu);
			_fsm.AddState(openWorld);
			_fsm.AddState(pause);
			_fsm.AddState(quitting);
			
			// 3. make all links
			_fsm.AddLink(boot,  splashScreen);
			_fsm.AddLink(splashScreen, mainMenu);
			
			_fsm.AddLink(mainMenu, settingsMenu);
			_fsm.AddLink(settingsMenu, mainMenu);
			
			_fsm.AddLink(mainMenu, openWorld);
			
			_fsm.AddLink(openWorld, pause);
			_fsm.AddLink(pause, openWorld);
			_fsm.AddLink(pause, quitting);
			
			_fsm.AddLink(quitting, mainMenu);
			
			// 4. Initialization
			_fsm.StartStateMachine((int)GameFlowExample.Boot);
		}

		[Button]
		private void Test(GameFlowExample target)
		{
			_fsm.TryChangeState((int)target);
		}
	}

	public enum GameFlowExample
	{
		Boot = 0,
		SplashScreen = 1,
		MainMenu = 2,
		SettingsMenu = 3,
		OpenWorld = 4,
		Pause = 5,
		Quitting = 6,
	}
}
