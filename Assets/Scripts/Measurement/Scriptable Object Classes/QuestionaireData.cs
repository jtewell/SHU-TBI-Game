using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


[CreateAssetMenu(fileName = "Questionaire Data", menuName = "Measurements/Questionaire Data", order = 2)]
public class QuestionaireData : DataBaseScriptableObject
{
    //Q0 - User ID
    public DataPair userID = new DataPair("entry.1172381183");

    //Q1 - Birthdate
    public DataPair birthMonth = new DataPair("entry.1715281728");
    public DataPair birthDay = new DataPair("entry.629660407");
    public DataPair birthYear = new DataPair("entry.982217643");

    //Q2 - Gender
    public DataPair gender = new DataPair ("entry.1484659508");

    //Q3 - Does user have history of TBI or repeated concussions?
    public DataPair hasTBIHistory = new DataPair("entry.1012726362");

    //Q4 - Did user experience mild-moderate concussions?
    public DataPair hadConcussions = new DataPair("entry.2103931096");

    //Q5 - How many concussions?
    public DataPair numberOfConcussions = new DataPair("entry.1340725638");

    //Q6 - Year of last concussion
    public DataPair yearLastConcussion = new DataPair("entry.336654288");

    //Q7 - Has TBI
    public DataPair hasTBI = new DataPair("entry.1653943654");

    //Q8 - How many TBIs
    public DataPair numberOfTBIs = new DataPair("entry.248416756");

    //Q9 - Year of last TBI
    public DataPair yearLastTBI = new DataPair("entry.2131642439");

    //Q10 - Can communinicate well
    public DataPair canCommunicateWell = new DataPair("entry.1654358928");

    //Q11 - Can keep up with conversations
    public DataPair canKeepUpConversations = new DataPair("entry.61572081");

    //Q12 - Can remember conversation details
    public DataPair canRememberConversations = new DataPair("entry.1296288566");

    //Q13 - Can remember steps for new tasks
    public DataPair canRememberSteps = new DataPair("entry.1428857749");

    //Q14 - Can follow multi-step instructions
    public DataPair canFollowInstructions = new DataPair("entry.123124654");

    //Q15 - Can follow instructions to location
    public DataPair canFollowLocationInstructions = new DataPair("entry.439547470");

    //Q16 - Get lost easily
    public DataPair getLostEasily = new DataPair("entry.2105550583");

    //Q17 - Get confused when more than one person talks
    public DataPair getConfusedPeopleTalk = new DataPair("entry.1062803565");

    //Q18 - Forget items to purchase at the store
    public DataPair forgetItemsToPurchase = new DataPair("entry.258204172");

    //Q19 - Can focus on a task
    public DataPair canFocusOnTask = new DataPair("entry.1402924908");

    //Q20 - Can change plans mid-day
    public DataPair canChangePlans = new DataPair("entry.1183053925");

    //Q21 - Can modify plans
    public DataPair canModifyPlans = new DataPair("entry.953435869");

    //Q22 - Can't remember details later
    public DataPair canRememberDetails = new DataPair("entry.1985824873");

    //Q23 - Can learn new tasks easily
    public DataPair canLearnTasks = new DataPair("entry.1100358298");

    //Q24 - I don't make sense talking
    public DataPair makeSenseTalking = new DataPair("entry.188062488");

    //Q25 - I ramble off topic
    public DataPair rambleOffTopic = new DataPair("entry.1061759675");

    //Q26 - I can go back on-topic
    public DataPair canGoBackOnTopic = new DataPair("entry.1629763265");

    //Q27 - Can manage money
    public DataPair canManageMoney = new DataPair("entry.798811690");

    //Q28 - Always buy things on shopping list
    public DataPair alwaysUseShoppingList = new DataPair("entry.1387695143");


}