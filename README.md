# testability-kata
A kata that I use to teach people how to convert legacy non-testable code to cleaner, 100% testable code. The different branches contain different "phases". The last phase has all tests implemented.

# Excercises
These excercises are carefully thought through to allow them to be used on real existing systems too (just follow the steps below on any system, and you'll have a testable system).

## 1. Get rid of static sickness (convert statics to non-statics)
This allows objects to actually have some form of "state". Essentially a static class is just "some functions and some global state operating somewhere" in your program. Statics also rarely actually save a lot of lines of code, and they can't be faked out.

## 2. Apply manual dependency injection to non-static class dependencies
Doing this is part of following the Open/Closed principle (the "O" in SOLID - see more below). By injecting dependencies in from the outside, we essentially allow tests to "fake out" the "internals" of our class if they want to, and provide their own versions of dependencies for this class.

_Note that the `MailSender` class dependency has not been converted yet in this step - that will be done in the next, since the constructor parameter for file name depends on the log level, which is only available by the time the `Log` method is called._

## 3. 
