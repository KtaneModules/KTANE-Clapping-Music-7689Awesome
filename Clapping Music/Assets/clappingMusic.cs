using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class clappingMusic: MonoBehaviour {

   public KMBombInfo bomb;
   public KMAudio audio;

   public KMSelectable ClapButton;
   public KMSelectable RestButton;

   public String[] pattern1;
   public String[] pattern2;
   public String[] pattern3;
   public String[] pattern4;
   public String[] pattern5;
   public String[] pattern6;
   public String[] pattern7;
   public String[] pattern8;
   public String[] pattern9;
   public String[] pattern10;
   public String[] pattern11;
   public String[] pattern12;

   private String[] correctPattern = new String[12];
   private String[] clapAnswer = new String[12];
   int correctPatternNumber = 0;
   int stage = 0;
   int correct = 0;

   static int ModuleIdCounter = 1;
   int moduleId;
   private bool ModuleSolved;

   void Awake() { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.

   moduleId = ModuleIdCounter++;
   /*foreach (KMSelectable object in keypad)
   {
      KMSelectable pressObject = object;
      object.OnInteract += delegate () { keypadPress(pressedObject); return false; };
   }
   */ 
   ClapButton.OnInteract += delegate () { PressClapButton(); return false; };
   RestButton.OnInteract += delegate () { PressRestButton(); return false; };

   }

   void Start() { //Shit that you calculate, usually a majority if not all of the module
      DeterminePattern();
   }

   void DeterminePattern(){
      if(bomb.IsIndicatorOn("BOB")){
      correctPatternNumber = 1;
      Debug.Log("Pattern 1 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern1[i];
         }

      } else if(bomb.GetBatteryCount()==0){
      correctPatternNumber = 2;   
      Debug.Log("Pattern 2 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern2[i];
         }

      } else if(bomb.GetPortCount(Port.PS2) == 1){
      correctPatternNumber = 3;      
      Debug.Log("Pattern 3 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern3[i];
         }

      } else if(bomb.GetSerialNumberNumbers().Last() == 0){
         correctPatternNumber = 4;
         Debug.Log("Pattern 4 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern4[i];
         }
      } else if(bomb.GetPortCount(Port.Parallel) == 1){
         correctPatternNumber = 5;
         Debug.Log("Pattern 5 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern5[i];
         }
      } else if(bomb.IsIndicatorOff("CLR")){
         correctPatternNumber = 6;
         Debug.Log("Pattern 6 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern6[i];
         }
      } else if((bomb.GetSerialNumberNumbers().Last() == 2) || (bomb.GetSerialNumberNumbers().Last() == 3) || (bomb.GetSerialNumberNumbers().Last() == 5) || (bomb.GetSerialNumberNumbers().Last() == 7)){
         correctPatternNumber = 7;
         Debug.Log("Pattern 7 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern7[i];
         }
      } else if(bomb.GetPortPlateCount() % 2 == 0){
         correctPatternNumber = 8;
         Debug.Log("Pattern 8 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern8[i];
         }
      } else if(bomb.IsIndicatorOff("CLR") || bomb.IsIndicatorOn("NSA")){
         correctPatternNumber = 9;
         Debug.Log("Pattern 9 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern9[i];
         }
      } else if(bomb.GetPortCount(Port.Parallel) == bomb.GetPortCount(Port.Serial)){
         correctPatternNumber = 10;
         Debug.Log("Pattern 10 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern10[i];
         }
      } else if(bomb.GetPortCount(Port.DVI) % 2 == 0){
         correctPatternNumber = 11;
         Debug.Log("Pattern 11 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern11[i];
         }
      } else {
         correctPatternNumber = 12;
         Debug.Log("Pattern 12 is the correct pattern.");
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern12[i];
         }   
      }
   }

  /*void LogClapAnswerContents() {
    Debug.Log("Current contents of clapAnswer array:");
    for (int i = 0; i < clapAnswer.Length; i++) {
        Debug.LogFormat("clapAnswer[{0}] = {1}", i, clapAnswer[i]);
    }
}*/

void LogCorrectPatternContents() {
    Debug.Log("The Correct Pattern:");
    for (int i = 0; i < correctPattern.Length; i++) {
        Debug.LogFormat("correctPattern[{0}] = {1}", i, correctPattern[i]);
    }
}

   void PressClapButton(){

      ClapButton.AddInteractionPunch();
      if (ModuleSolved){
         return;
      }

      audio.PlaySoundAtTransform("CLAP SOUND", transform);

      Debug.Log("Clap Button was pressed!");
      Debug.LogFormat("This is the {0}th button pressed.", stage+1);
      clapAnswer[stage] = "Clap";
      stage += 1;

      if (stage == 12){
         LogCorrectPatternContents();

         for(int j = 0; j<12; j++){
            if (string.Equals(clapAnswer[j], correctPattern[j], StringComparison.OrdinalIgnoreCase)){
         correct += 1;
      }

      }
         if (correct == 12){
            Solve();
         } else{
            Strike();
            Array.Clear(clapAnswer,0, clapAnswer.Length);
            Debug.Log("Reset!");
         }
         stage = 0;
         correct = 0;
      }
      }

   void PressRestButton(){

      RestButton.AddInteractionPunch();
      if (ModuleSolved){
         return;
      }

      audio.PlaySoundAtTransform("REST SOUND", transform);

      Debug.Log("Rest Button was pressed!");
      Debug.LogFormat("This is the {0}th button pressed.", stage+1);
      clapAnswer[stage] = "Rest";
      stage += 1;

      if (stage == 12){
         LogCorrectPatternContents() ;

         for(int a = 0; a<12; a++){
            if (string.Equals(clapAnswer[a], correctPattern[a], StringComparison.OrdinalIgnoreCase)){
         correct += 1;
      }
         }

         if (correct == 12){
            Solve();
         } else {
            Strike();
            Array.Clear(clapAnswer, 0, clapAnswer.Length);
            Debug.Log("Reset!");
         }
         correct = 0;
         stage = 0;
      }
   }


   void Solve() {
      audio.PlaySoundAtTransform("SOLVED SOUND", transform);
      GetComponent<KMBombModule>().HandlePass();
      Debug.Log("Module Solved!");
   }

   void Strike() {
      GetComponent<KMBombModule>().HandleStrike();
      Debug.Log("Strike! Try again.");
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}
