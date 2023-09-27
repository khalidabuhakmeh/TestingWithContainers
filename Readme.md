# Using TestContainers for Integration Tests

This repository contains strategies for integration testing with [TestContainers](https://dotnet.testcontainers.org/) and XUnit. Specifically, these tests show how to use XUnit's facilities to provide dependencies on several levels:

- Dependency per test 
- Dependency per test class
- Dependency per test collection

These three strategies provide varying levels of execution speed and test isolation.

The strategy you choose will be based on your scenarios and preferred outcome.

As a general rule, it's best to start with **dependency per test**, as it provides the most isolation between tests, but it can be time-intensive as your test suite grows.

The other strategies can be powerful in speeding up your test runs, but require thinking about initialization and clean-up strategies as state can "leak" between tests.

## Getting Started

You'll need the following to run these tests.

- .NET 8 SDK
- JetBrains Rider (optional)

Running the tests in the JetBrains Rider test runner will show how tests are isolated, as the **Container Id** is printed to the test runner output window. Also note the time it takes to execute each collection of tests.

On my machine, the tests take about this much time:

- Dependency per test: 3 tests @ 13 seconds.
- Dependency per test class: 3 tests @ 9 seconds
- Dependency per test collection: 6 tests @ 4 seconds

Each strategy is running identical tests.