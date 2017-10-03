# testability-kata
A kata that I use to teach people how to convert legacy non-testable code to cleaner, 100% testable code. The different branches contain different "phases". The last phase has all tests implemented.

This kata does not teach about test conventions like AAA (Arrange Act Assert) or how to name your tests. It doesn't dictate whether to use BDD (Business Driven Development) style testing or similar. You still have to agree on your team how to structure that. The kata focuses on the "universal" things that apply to all systems and all teams.

# Excercises
These excercises are carefully thought through to allow them to be used on real existing systems too (just follow the steps below on any system, and you'll have a testable system).

## Making the system testable with non-intrusive changes
First we need to make our system testable, and the key point here is that all of the below changes can be performed in a non-intrusive manner (so that we don't break anything).

To see the original non-testable code: https://github.com/ffMathy/testability-kata/blob/master/src/TestabilityKata/Program.cs

### 1. Get rid of static sickness (convert statics to non-statics)
This allows objects to actually have some form of "state". Essentially a static class is just "some functions and some global state operating somewhere" in your program. Statics also rarely actually save a lot of lines of code, and they can't be faked out.

_If your static dependency has too many references, it can be hard to "just" convert into non-statics. In this case, take your existing static method (let's say it's called `SendMail`) and rename it into `SendMailStatic` so that all references will point to this method for backward compatibility. Then take the body of this method and move it into a new non-static method called `SendMail`. Finally, change the `SendMailStatic` method so that it instantiates a new instance of itself and calls the non-static `SendMail` function on that instance. This allows you to take "baby steps" in getting rid of static sickness instead of doing it all. Just remember that static functions can't be faked out._

To see this change: https://github.com/ffMathy/testability-kata/compare/step-1

### 2. Apply manual dependency injection to non-static class dependencies where only one instance of a dependency per class is required
Doing this is part of following the Open/Closed principle (the "O" in SOLID). By injecting dependencies in from the outside, we essentially allow tests to "fake out" the "internals" of our class if they want to, and provide their own versions of dependencies for this class. As an example, it allows us to provide our own `Logger` for our `Program`, or our own `MailSender` for our `Logger`, even if `Logger` or `MailSender` was a third party NuGet package that we couldn't change.

_Note that the `Logger`'s `CustomFileWriter` class dependency has not been converted yet in this step - that will be done in the next step, since the constructor parameter for file name depends on the log level, which is only available by the time the `Log` method is called._

To see this change: https://github.com/ffMathy/testability-kata/compare/step-1...step-2

### 3. Apply manual dependency injection via factories for scenarios where creation of objects at runtime is required
This is about continuing step 2, but for the `Logger`'s `CustomFileWriter` dependency. Since it is created in the `Log` method and the file name to log to depends on the log level given in this method, we can't just inject a `CustomFileWriter` into the constructor. Here, we instead create a `CustomFileWriterFactory` which we then inject, and this factory then injects the dependencies required for creating a `CustomFileWriter`.

To see this change: https://github.com/ffMathy/testability-kata/compare/step-2...step-3

### 4. Extract interfaces from classes and rely on what objects "do" - not what they "are"
This is about applying the Dependency Inversion principle (the D in SOLID) and making our program more losely coupled. A dependency declared as a class is tightly coupled to "what that class is" (for instance that it's a `Tiger` class). If it was declared as an interface instead - for instance `IAnimal` which then had a `Bite` method, then it would instead just be coupled to "something that can bite".

Within testability this allows us to make "fake" implementations of dependencies (for instance, a fake `MailSender` which doesn't actually send e-mails when running unit tests). More on this later when we get to writing the tests.

To see this change: https://github.com/ffMathy/testability-kata/compare/step-3...step-4

## Testing the system
It should be noted that these tests only focus on the "unique" scenarios where something needs to be handled differently from step to step. In general, both negative and positive outcomes of tests (throwing exceptions and/or passing) and most input scenarios should be tested.

### Unit testing the `Program` class

#### 5. Test that the program sends an e-mail about it starting, when it starts up
Technically here we could create our own `FakeEmailSender` class, pass it to our `Program` instance when instantiating it, and then having this `FakeEmailSender` report what arguments its `SendMail` method was called with, and pass the test if these arguments are correct and the method was indeed called.

However, for now we will skip all this and use `NSubstitute`, which can magically at runtime (it uses the `TypeBuilder` class in C# for this) create a new class that implements a given interface, and record calls and change its runtime behavior.

So to summarize, this step is about making a fake `IEmailSender` at runtime using `NSubstitute`, passing this into the `Program` instance as a dependency, and then (after executing `Program.Run()`), we check that the `IEmailSender.SendMail` method was called with the right arguments.

To see this change: https://github.com/ffMathy/testability-kata/compare/step-4...step-5

#### 6. Test that the program logs an error if one of its dependencies throws one on startup
Similarly to step 5, we here need to have `NSubstitute` create a fake dependency which throws an exception. We then run the program, and need to make sure that the `Logger`'s `Log` method was called with a log level of `Error`.

To see this change: https://github.com/ffMathy/testability-kata/compare/step-5...step-6

### Unit testing the `MailSender` class

#### 7. Test that the mail sender throws an exception if the e-mail is invalid (doesn't contain a `@`)
It turns out that this class has no dependencies and was already testable all along - so this should be easy. No faking required here, and we can just invoke the method directly and put an `ExpectedExceptionAttribute` on our test to describe that it should _pass_ instead of failing when a specific exception is thrown.

To see this change: https://github.com/ffMathy/testability-kata/compare/step-6...step-7

## Cleaning up the code
