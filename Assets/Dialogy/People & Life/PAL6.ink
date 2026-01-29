-> main
VAR correct = "To be under the weather"
VAR chosen = ""

=== main ===
Jimmy the Joker : Which idiom means to feel slightly ill or unwell?
    +   To beat around the bush
        ~ chosen = "To beat around the bush"
        #WRONG_CHOICE
        -> check_choice
        
    + To be in the same boat
        ~ chosen = "To be in the same boat"
        #WRONG_CHOICE
        -> check_choice
        
    + To be under the weather
        ~ chosen = "To be under the weather"
        #CORRECT_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Get well soon! Correct! | Incorrect. Think about rain and clouds.}
-> END