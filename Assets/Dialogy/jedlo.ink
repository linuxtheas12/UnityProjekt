-> main
VAR correct = "Piece of cake"
VAR chosen = ""

=== main ===
Mythical Wise Tree : jedlo druyhy level Which idiom means “Something very easy to do”?
    + Hit the nail on the head
        ~ chosen = "Hit the nail on the head"
        #WRONG_CHOICE
        -> check_choice
        
    + Piece of cake
        ~ chosen = "Piece of cake"
        #CORRECT_CHOICE
        -> check_choice
        
    + Raining cats and dogs
        ~ chosen = "Raining cats and dogs"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct:  Yes! Piece of cake! That was easy, just like me catching a ball! |  Hmm… not quite! Think about something easy!}
-> END