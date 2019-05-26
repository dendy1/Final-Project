using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
   //player
   [SerializeField] private CharacterPanel attackPanel;
   
   //enemy
   [SerializeField] private CharacterPanel defencePanel;

   private GameObject _attack;
   private GameObject _defence;

   private void Awake()
   {
      var obj = Scenes.GetSceneParameters;
      
   }
}

[Serializable]
public struct CharacterPanel
{
   public Text nameTextField;
   public Text levelTextField;
   public Text hpTextField;
   public Image HealthBarImage;

}
