-> main
VAR correct = "To let the cat out of the bag"
VAR chosen = ""

=== main ===
Jimmy the Joker : Which idiom means to accidentally reveal a secret?
    + To let the cat out of the bag
        ~ chosen = "To let the cat out of the bag"
        #CORRECT_CHOICE
        -> check_choice
        
    + To break the ice
        ~ chosen = "To break the ice"
        #WRONG_CHOICE
        -> check_choice
        
    + To be under the weather
        ~ chosen = "To be under the weather"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Oops! The secret is out! Correct! | No, the secret is still hidden!}
-> END