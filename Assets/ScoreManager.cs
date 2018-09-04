using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    // put these in a score controller class??
    public CardType CheckForTrump(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard, ClueCard HolmesClueCard, ClueCard MoriartyClueCard, int Effect)
    {
        switch (Effect)
        {
            case 0:
                return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
            case 1:
                return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
            case 2:
                return M2Case(HolmesCrimeCard, MoriartyCrimeCard, HolmesClueCard, MoriartyClueCard);
            case 3:
                return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
            case 4:
                return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
            case 5:
                return M5Case(HolmesCrimeCard, MoriartyCrimeCard);
            case 6:
                return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
            case 7:
                return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
            case 8:
                return M8Case(HolmesCrimeCard, MoriartyCrimeCard);
            case 9:
                return M9Case(HolmesCrimeCard, MoriartyCrimeCard, HolmesClueCard, MoriartyClueCard);
            case 10:
                return M10Case(HolmesCrimeCard, MoriartyCrimeCard);
            case 11:
                return M11Case(HolmesCrimeCard, MoriartyCrimeCard);

        }
        Debug.LogError("Case not in system");
        return 0;


    }

    CardType M11Case(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        if (HolmesCrimeCard.ThisCardType == CardType.Blue || MoriartyCrimeCard.ThisCardType == CardType.Blue)
        {
            int HolmesModifier = 0;
            if (HolmesCrimeCard.ThisCardType == CardType.Blue) { HolmesModifier = -3; }
            int HolmesCrimeCardNumber = HolmesCrimeCard.Number + HolmesModifier;

            int MoriartyModifier = 0;
            if (MoriartyCrimeCard.ThisCardType == CardType.Blue) { MoriartyModifier = -3; }
            int MoriartyCrimeCardNumber = MoriartyCrimeCard.Number + MoriartyModifier;

            //M wins ties 
            if (HolmesModifier == 0)
            {
                if (HolmesCrimeCardNumber > MoriartyCrimeCardNumber)
                {
                    // check for wrap around effect 
                    switch (MoriartyCrimeCardNumber)
                    {
                        case -2:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case -1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 0:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                    }
                    return HolmesCrimeCard.ThisCardType;
                }
                else
                {
                    switch (HolmesCrimeCardNumber)
                    {
                        //Acts like a 1. Could possibly be not true
                        case -2:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case -1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 0:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;


                        case 1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                            break;
                    }
                    return MoriartyCrimeCard.ThisCardType;
                }
            }
            else
            {
                //H wins ties 
                if (HolmesCrimeCardNumber >= MoriartyCrimeCardNumber)
                {
                    // check for wrap around effect 
                    switch (MoriartyCrimeCardNumber)
                    {
                        case -2:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case -1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 0:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;

                        case 1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                    }
                    return HolmesCrimeCard.ThisCardType;
                }
                else
                {
                    switch (HolmesCrimeCardNumber)
                    {
                        case -2:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case -1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 0:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;

                        case 1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                            break;
                    }
                    return MoriartyCrimeCard.ThisCardType;
                }
            }
        }
        else
        {
            return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
        }
    }

    CardType M10Case(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        if (HolmesCrimeCard.ThisCardType == CardType.Red || MoriartyCrimeCard.ThisCardType == CardType.Red)
        {
            int HolmesModifier = 0;
            if (HolmesCrimeCard.ThisCardType == CardType.Red) { HolmesModifier = -3; }
            int HolmesCrimeCardNumber = HolmesCrimeCard.Number + HolmesModifier;

            int MoriartyModifier = 0;
            if (MoriartyCrimeCard.ThisCardType == CardType.Red) { MoriartyModifier = -3; }
            int MoriartyCrimeCardNumber = MoriartyCrimeCard.Number + MoriartyModifier;

            //M wins ties 
            if (HolmesModifier == 0)
            {
                if (HolmesCrimeCardNumber > MoriartyCrimeCardNumber)
                {
                    // check for wrap around effect 
                    switch (MoriartyCrimeCardNumber)
                    {
                        case -1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 0:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                    }
                    return HolmesCrimeCard.ThisCardType;
                }
                else
                {
                    switch (HolmesCrimeCardNumber)
                    {
                        //Acts like a 1. Could possibly be not true
                        case -1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 0:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;


                        case 1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                            break;
                    }
                    return MoriartyCrimeCard.ThisCardType;
                }
            }
            else
            {
                //H wins ties 
                if (HolmesCrimeCardNumber >= MoriartyCrimeCardNumber)
                {
                    // check for wrap around effect 
                    switch (MoriartyCrimeCardNumber)
                    {
                        case -1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 0:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;

                        case 1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                    }
                    return HolmesCrimeCard.ThisCardType;
                }
                else
                {
                    switch (HolmesCrimeCardNumber)
                    {
                        //Acts like a 1. Could possibly be not true
                        case -1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 0:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;

                        case 1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                            break;
                    }
                    return MoriartyCrimeCard.ThisCardType;
                }
            }
        }
        else
        {
            return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
        }
    }

    CardType M9Case(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard, ClueCard HolmesClueCard, ClueCard MoriartyClueCard)
    {
        if (HolmesCrimeCard.ThisCardType == CardType.Blue || MoriartyCrimeCard.ThisCardType == CardType.Blue ||
            HolmesClueCard.ThisCardType == CardType.Blue || MoriartyClueCard.ThisCardType == CardType.Blue)
        {
            int HolmesModifier = 0;
            if (HolmesCrimeCard.ThisCardType == CardType.Yellow) { HolmesModifier = 2; }
            int HolmesCrimeCardNumber = HolmesCrimeCard.Number + HolmesModifier;

            int MoriartyModifier = 0;
            if (MoriartyCrimeCard.ThisCardType == CardType.Yellow) { MoriartyModifier = 2; }
            int MoriartyCrimeCardNumber = MoriartyCrimeCard.Number + MoriartyModifier;

            //M wins ties 
            if (HolmesModifier == 0)
            {
                if (HolmesCrimeCardNumber > MoriartyCrimeCardNumber)
                {
                    // check for wrap around effect 
                    switch (MoriartyCrimeCardNumber)
                    {
                        case 1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                    }
                    return HolmesCrimeCard.ThisCardType;
                }
                else
                {
                    switch (HolmesCrimeCardNumber)
                    {
                        case 1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                            break;
                    }
                    return MoriartyCrimeCard.ThisCardType;
                }
            }
            else
            {
                //H wins ties 
                if (HolmesCrimeCardNumber >= MoriartyCrimeCardNumber)
                {
                    // check for wrap around effect 
                    switch (MoriartyCrimeCardNumber)
                    {
                        case 1:
                            if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                            break;
                    }
                    return HolmesCrimeCard.ThisCardType;
                }
                else
                {
                    switch (HolmesCrimeCardNumber)
                    {
                        case 1:
                            if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 2:
                            if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                            break;
                        case 3:
                            if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                            break;
                    }
                    return MoriartyCrimeCard.ThisCardType;
                }
            }
        }
        else
        {
            return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
        }


    }

    CardType M8Case(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        if (HolmesCrimeCard.Number > MoriartyCrimeCard.Number)
        {
            // check for wrap around effect 
            switch (MoriartyCrimeCard.Number)
            {
                case 1:
                    if (HolmesCrimeCard.Number > 13) { return HolmesCrimeCard.ThisCardType; }
                    break;
                case 2:
                    if (HolmesCrimeCard.Number > 14) { return HolmesCrimeCard.ThisCardType; }
                    break;
                case 3:
                    if (HolmesCrimeCard.Number > 15) { return HolmesCrimeCard.ThisCardType; }
                    break;
            }
            return MoriartyCrimeCard.ThisCardType;
        }
        else
        {
            switch (HolmesCrimeCard.Number)
            {
                case 1:
                    if (MoriartyCrimeCard.Number > 13) { return MoriartyCrimeCard.ThisCardType; }
                    break;
                case 2:
                    if (MoriartyCrimeCard.Number > 14) { return MoriartyCrimeCard.ThisCardType; }
                    break;
                case 3:
                    if (MoriartyCrimeCard.Number > 15) { return MoriartyCrimeCard.ThisCardType; }
                    break;
            }
            return HolmesCrimeCard.ThisCardType;
        }
    }

    CardType M5Case(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        if (HolmesCrimeCard.ThisCardType == CardType.Green && MoriartyCrimeCard.ThisCardType == CardType.Green)
        {
            if (HolmesCrimeCard.Number > MoriartyCrimeCard.Number) { return HolmesCrimeCard.ThisCardType; }
            else { return MoriartyCrimeCard.ThisCardType; }
        }
        else if (HolmesCrimeCard.ThisCardType == CardType.Green)
        {
            int HolmesCrimeCardNumber = HolmesCrimeCard.Number + 2;
            if (HolmesCrimeCardNumber > MoriartyCrimeCard.Number)
            {
                // check for wrap around effect 
                switch (MoriartyCrimeCard.Number)
                {
                    case 1:
                        if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                }
                return HolmesCrimeCard.ThisCardType;
            }
            else
            {
                switch (HolmesCrimeCardNumber)
                {
                    case 1:
                        if (MoriartyCrimeCard.Number > 13) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (MoriartyCrimeCard.Number > 14) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (MoriartyCrimeCard.Number > 15) { return HolmesCrimeCard.ThisCardType; }
                        break;
                }
                return MoriartyCrimeCard.ThisCardType;
            }
        }
        else if (MoriartyCrimeCard.ThisCardType == CardType.Green)
        {
            int MoriartyCrimeCardNumber = MoriartyCrimeCard.Number + 2;
            if (HolmesCrimeCard.Number > MoriartyCrimeCardNumber)
            {
                // check for wrap around effect 
                switch (MoriartyCrimeCardNumber)
                {
                    case 1:
                        if (HolmesCrimeCard.Number > 13) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (HolmesCrimeCard.Number > 14) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (HolmesCrimeCard.Number > 15) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                }
                return HolmesCrimeCard.ThisCardType;
            }
            else
            {
                switch (HolmesCrimeCard.Number)
                {
                    case 1:
                        if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                        break;
                }
                return MoriartyCrimeCard.ThisCardType;
            }
        }
        else
        {
            return (normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard));
        }
    }

    CardType M2Case(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard, ClueCard HolmesClueCard, ClueCard MoriartyClueCard)
    {
        //Both Have the condition
        if (HolmesClueCard.Number < HolmesCrimeCard.Number && MoriartyClueCard.Number < MoriartyCrimeCard.Number)
        {
            int HolmesCrimeCardNumber = HolmesCrimeCard.Number - 3;
            int MoriartyCrimeCardNumber = MoriartyCrimeCard.Number - 3;
            if (HolmesCrimeCardNumber > MoriartyCrimeCardNumber)
            {
                // check for wrap around effect 
                switch (MoriartyCrimeCardNumber)
                {
                    case 1:
                        if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                }
                return HolmesCrimeCard.ThisCardType;
            }
            else
            {
                switch (HolmesCrimeCardNumber)
                {
                    case 1:
                        if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                        break;
                }
                return MoriartyCrimeCard.ThisCardType;
            }
        }
        else if (HolmesClueCard.Number < HolmesCrimeCard.Number)
        {
            int HolmesCrimeCardNumber = HolmesCrimeCard.Number - 3;
            // H wins ties
            if (HolmesCrimeCardNumber >= MoriartyCrimeCard.Number)
            {
                // check for wrap around effect 
                switch (MoriartyCrimeCard.Number)
                {
                    case 1:
                        if (HolmesCrimeCardNumber > 13) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (HolmesCrimeCardNumber > 14) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (HolmesCrimeCardNumber > 15) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                }
                return HolmesCrimeCard.ThisCardType;
            }
            else
            {
                switch (HolmesCrimeCardNumber)
                {
                    case 1:
                        if (MoriartyCrimeCard.Number > 13) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (MoriartyCrimeCard.Number > 14) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (MoriartyCrimeCard.Number > 15) { return HolmesCrimeCard.ThisCardType; }
                        break;
                }
                return MoriartyCrimeCard.ThisCardType;
            }
        }
        else if (MoriartyClueCard.Number < MoriartyCrimeCard.Number)
        {
            int MoriartyCrimeCardNumber = MoriartyCrimeCard.Number - 3;
            // M wins ties
            if (HolmesCrimeCard.Number > MoriartyCrimeCardNumber)
            {
                // check for wrap around effect 
                switch (MoriartyCrimeCardNumber)
                {
                    case 1:
                        if (HolmesCrimeCard.Number > 13) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (HolmesCrimeCard.Number > 14) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (HolmesCrimeCard.Number > 15) { return MoriartyCrimeCard.ThisCardType; }
                        break;
                }
                return HolmesCrimeCard.ThisCardType;
            }
            else
            {
                switch (HolmesCrimeCard.Number)
                {
                    case 1:
                        if (MoriartyCrimeCardNumber > 13) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 2:
                        if (MoriartyCrimeCardNumber > 14) { return HolmesCrimeCard.ThisCardType; }
                        break;
                    case 3:
                        if (MoriartyCrimeCardNumber > 15) { return HolmesCrimeCard.ThisCardType; }
                        break;
                }
                return MoriartyCrimeCard.ThisCardType;
            }
        }
        else
        {
            return normalCrimeCase(HolmesCrimeCard, MoriartyCrimeCard);
        }


    }

    CardType normalCrimeCase(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        if (HolmesCrimeCard.Number > MoriartyCrimeCard.Number)
        {
            // check for wrap around effect 
            switch (MoriartyCrimeCard.Number)
            {
                case 1:
                    if (HolmesCrimeCard.Number > 13) { return MoriartyCrimeCard.ThisCardType; }
                    break;
                case 2:
                    if (HolmesCrimeCard.Number > 14) { return MoriartyCrimeCard.ThisCardType; }
                    break;
                case 3:
                    if (HolmesCrimeCard.Number > 15) { return MoriartyCrimeCard.ThisCardType; }
                    break;
            }
            return HolmesCrimeCard.ThisCardType;
        }
        else
        {
            switch (HolmesCrimeCard.Number)
            {
                case 1:
                    if (MoriartyCrimeCard.Number > 13) { return HolmesCrimeCard.ThisCardType; }
                    break;
                case 2:
                    if (MoriartyCrimeCard.Number > 14) { return HolmesCrimeCard.ThisCardType; }
                    break;
                case 3:
                    if (MoriartyCrimeCard.Number > 15) { return HolmesCrimeCard.ThisCardType; }
                    break;
            }
            return MoriartyCrimeCard.ThisCardType;
        }
    }

    // Check to see who has the highest card with trump in play 
    // first check who has trump and then if either both do or do not check for highest card
    public bool CheckForHolmesWin(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard, ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard, int Effect)
    {

        switch (Effect)
        {
            case 0:
                return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
            case 1:
                return M1Case(Trump, HolmesClueCard, MoriartyClueCard);
            case 2:
                return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
            case 3:
                return M3Case(Trump, HolmesClueCard, MoriartyClueCard, HolmesCrimeCard, MoriartyCrimeCard);
            case 4:
                return M4Case(Trump, HolmesClueCard, MoriartyClueCard, HolmesCrimeCard, MoriartyCrimeCard);
            case 5:
                return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
            case 6:
                return M6Case(Trump, HolmesClueCard, MoriartyClueCard, HolmesCrimeCard, MoriartyCrimeCard);
            case 7:
                return M7Case(Trump, HolmesClueCard, MoriartyClueCard);
            case 8:
                return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
            case 9:
                return M9Case2(Trump, HolmesClueCard, MoriartyClueCard, HolmesCrimeCard, MoriartyCrimeCard);
            case 10:
                return M10Case2(Trump, HolmesClueCard, MoriartyClueCard);
            case 11:
                return M11Case2(Trump, HolmesClueCard, MoriartyClueCard);

        }
        Debug.LogError("Case not in system");
        return false;
        // check if both have trump

    }

    bool M11Case2(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard)
    {
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump ||
           HolmesClueCard.ThisCardType != Trump && MoriartyClueCard.ThisCardType != Trump)
        {
            int HolmesModifier = 0;
            if (HolmesClueCard.ThisCardType == CardType.Green) { HolmesModifier = 5; }
            int HolmesClueCardNumber = HolmesClueCard.Number + HolmesModifier;

            int MoriartyModifier = 0;
            if (MoriartyClueCard.ThisCardType == CardType.Green) { MoriartyModifier = 5; }
            int MoriartyClueCardNumber = MoriartyClueCard.Number + MoriartyModifier;

            // H wins ties
            if (MoriartyModifier == 0) { return (HolmesClueCardNumber > MoriartyClueCardNumber); }
            //M wins ties
            else if (HolmesModifier == 0) { return (HolmesClueCardNumber >= MoriartyClueCardNumber); }
            else { return (HolmesClueCardNumber > MoriartyClueCardNumber); }

        }
        else
        {
            return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
        }
    }

    bool M10Case2(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard)
    {
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump ||
           HolmesClueCard.ThisCardType != Trump && MoriartyClueCard.ThisCardType != Trump)
        {
            int HolmesModifier = 0;
            if (HolmesClueCard.ThisCardType == CardType.Yellow) { HolmesModifier = 5; }
            int HolmesClueCardNumber = HolmesClueCard.Number + HolmesModifier;

            int MoriartyModifier = 0;
            if (MoriartyClueCard.ThisCardType == CardType.Yellow) { MoriartyModifier = 5; }
            int MoriartyClueCardNumber = MoriartyClueCard.Number + MoriartyModifier;

            // H wins ties
            if (MoriartyModifier == 0) { return (HolmesClueCardNumber > MoriartyClueCardNumber); }
            //M wins ties
            else if (HolmesModifier == 0) { return (HolmesClueCardNumber >= MoriartyClueCardNumber); }
            else { return (HolmesClueCardNumber > MoriartyClueCardNumber); }

        }
        else
        {
            return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
        }
    }

    bool M9Case2(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard, ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump ||
           HolmesClueCard.ThisCardType != Trump && MoriartyClueCard.ThisCardType != Trump)
        {
            if (HolmesCrimeCard.ThisCardType == CardType.Blue || MoriartyCrimeCard.ThisCardType == CardType.Blue ||
                HolmesClueCard.ThisCardType == CardType.Blue || MoriartyClueCard.ThisCardType == CardType.Blue)
            {
                int HolmesModifier = 0;
                if (HolmesCrimeCard.ThisCardType == CardType.Yellow) { HolmesModifier = 2; }
                int HolmesClueCardNumber = HolmesCrimeCard.Number + HolmesModifier;

                int MoriartyModifier = 0;
                if (MoriartyCrimeCard.ThisCardType == CardType.Yellow) { MoriartyModifier = 2; }
                int MoriartyClueCardNumber = MoriartyCrimeCard.Number + MoriartyModifier;

                // H wins ties
                if (MoriartyModifier == 0) { return (HolmesClueCardNumber > MoriartyClueCardNumber); }
                //M wins ties
                else if (HolmesModifier == 0) { return (HolmesClueCardNumber >= MoriartyClueCardNumber); }
                else { return (HolmesClueCardNumber > MoriartyClueCardNumber); }
            }
        }
        return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);

    }

    bool M7Case(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard)
    {
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump ||
            HolmesClueCard.ThisCardType != Trump && MoriartyClueCard.ThisCardType != Trump)
        {
            int HolmesModifier = 0;
            if (HolmesClueCard.ThisCardType == CardType.Red) { HolmesModifier = 5; }
            int HolmesClueCardNumber = HolmesClueCard.Number + HolmesModifier;

            int MoriartyModifier = 0;
            if (MoriartyClueCard.ThisCardType == CardType.Red) { MoriartyModifier = 5; }
            int MoriartyClueCardNumber = MoriartyClueCard.Number + MoriartyModifier;

            // H wins ties
            if (MoriartyModifier == 0) { return (HolmesClueCardNumber > MoriartyClueCardNumber); }
            //M wins ties
            else if (HolmesModifier == 0) { return (HolmesClueCardNumber >= MoriartyClueCardNumber); }
            else { return (HolmesClueCardNumber > MoriartyClueCardNumber); }

        }
        else
        {
            return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
        }
    }

    bool M6Case(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard, ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump ||
           HolmesClueCard.ThisCardType != Trump && MoriartyClueCard.ThisCardType != Trump)
        {
            int HolmesModifier = 0;
            if (HolmesClueCard.ThisCardType == CardType.Blue)
            {
                HolmesModifier++;
                if (HolmesCrimeCard.ThisCardType == CardType.Blue) { HolmesModifier++; }
            }
            int HolmesClueCardNumber = HolmesClueCard.Number + HolmesModifier;

            int MoriartyModifier = 0;
            if (MoriartyClueCard.ThisCardType == CardType.Blue)
            {
                MoriartyModifier++;
                if (MoriartyCrimeCard.ThisCardType == CardType.Blue) { MoriartyModifier++; }
            }
            int MoriartyClueCardNumber = MoriartyClueCard.Number + MoriartyModifier;

            if (MoriartyModifier == 0) { return (HolmesClueCardNumber > MoriartyClueCardNumber); }
            else if (HolmesModifier == 0) { return (HolmesClueCardNumber >= MoriartyClueCardNumber); }
            else { return (HolmesClueCardNumber > MoriartyClueCardNumber); }

        }
        else
        {
            return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
        }
    }

    bool M4Case(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard, ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        //both have trump or both do not 
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump ||
            HolmesClueCard.ThisCardType != Trump && MoriartyClueCard.ThisCardType != Trump)
        {
            //Moriarty Lost Trump
            if (Trump == HolmesCrimeCard.ThisCardType)
            {
                int MoriartyClueCardNumber = MoriartyClueCard.Number + 5;
                //M wins ties
                return (HolmesClueCard.Number > MoriartyClueCardNumber);
            }
            //Holmes Lost Trump
            else
            {
                int HolmesClueCardNumber = MoriartyClueCard.Number + 5;
                //H wins ties
                return (HolmesClueCardNumber >= MoriartyClueCard.Number);
            }
        }
        else
        {
            return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
        }
    }

    bool M3Case(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard, ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {
        //both have trump or both do not 
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump ||
            HolmesClueCard.ThisCardType != Trump && MoriartyClueCard.ThisCardType != Trump)
        {
            // both have the condition
            if (HolmesClueCard.ThisCardType == MoriartyCrimeCard.ThisCardType &&
                MoriartyClueCard.ThisCardType == HolmesCrimeCard.ThisCardType)
            {
                return (HolmesClueCard.Number > MoriartyClueCard.Number);
            }
            else if (HolmesClueCard.ThisCardType == MoriartyCrimeCard.ThisCardType)
            {
                int HolmesClueNumber = HolmesClueCard.Number + 5;
                //H wins ties
                return (HolmesClueNumber >= MoriartyClueCard.Number);
            }
            else if (MoriartyClueCard.ThisCardType == HolmesCrimeCard.ThisCardType)
            {
                int MorartyClueCardNumber = MoriartyClueCard.Number + 5;
                //M wins ties
                return (HolmesClueCard.Number > MorartyClueCardNumber);
            }
            else
            {
                return (HolmesClueCard.Number > MoriartyClueCard.Number);
            }
        }
        else
        {
            return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
        }
    }

    bool M1Case(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard)
    {
        if (HolmesClueCard.Number < 5 || MoriartyClueCard.Number < 5)
        {
            //All have trump condition
            if (HolmesClueCard.Number < 5 && MoriartyClueCard.Number < 5 || HolmesClueCard.Number < 5 && MoriartyClueCard.ThisCardType == Trump ||
                MoriartyClueCard.Number < 5 && HolmesClueCard.ThisCardType == Trump)
            {
                return (HolmesClueCard.Number > MoriartyClueCard.Number);
            }
            //Holmes has trump
            else if (HolmesClueCard.Number < 5)
            {
                return true;
            }
            else if (MoriartyClueCard.Number < 5)
            {
                return false;
            }
            else
            {
                return (HolmesClueCard.Number > MoriartyClueCard.Number);
            }
        }
        else
        {
            return NormalCase(Trump, HolmesClueCard, MoriartyClueCard);
        }
    }

    bool NormalCase(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard)
    {
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump)
        {
            return (HolmesClueCard.Number > MoriartyClueCard.Number);
        }
        // check if both only player has trump
        else if (HolmesClueCard.ThisCardType == Trump)
        {
            return true;
        }
        // check if both only AI has trump
        else if (MoriartyClueCard.ThisCardType == Trump)
        {
            return false;
        }

        // check if neither has trump
        else
        {
            return (HolmesClueCard.Number > MoriartyClueCard.Number);
        }
    }

}
