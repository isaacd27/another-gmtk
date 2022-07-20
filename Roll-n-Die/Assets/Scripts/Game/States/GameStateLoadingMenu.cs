using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateLoadingMenu : GameStateLoadingBaseState
{
    protected override GameStateDefinition NextState => m_menuState;
    [SerializeField]
    private GameStateDefinition m_menuState;

    // I want the main menu to be display during the black screen
    protected override void OnLoadingCompleted()
    {    }

    protected override void Load() 
    {
        SetState(NextState);
    }
}