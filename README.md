# MovieRental Exercise

This is a dummy representation of a movie rental system.
Can you help us fix some issues and implement missing features?

 * The app is throwing an error when we start, please help us. Also, tell us what caused the issue.
   * The error occurs because a **Singleton** service (IRentalFeatures) depends on a **Scoped** service (MovieRentalDbContext). In ASP.NET Core, scoped services are created per HTTP request and disposed afterward, while singletons live for the entire application lifetime. Allowing a singleton to hold a scoped dependency would cause it to reference a disposed DbContext. To prevent this, the DI container blocks the application at startup. Changing IRentalFeatures to Scoped aligns the lifetimes correctly, ensuring both services are created and disposed within the same request scope.
 * The rental class has a method to save, but it is not async, can you make it async and explain to us what is the difference?
   * The Save method was made asynchronous to prevent blocking the executing thread during database operations. By using SaveChangesAsync, the application can release the thread while waiting for I/O, improving scalability and performance under concurrent load. A synchronous implementation would block the thread until the database operation completes, reducing overall throughput.
 * Please finish the method to filter rentals by customer name, and add the new endpoint.
   * The method was implemented without using async because it only builds and returns an IQueryable, which represents a deferred query and does not trigger any database access. Since the project uses OData, the query execution, filtering, and materialization are handled later by the OData pipeline, which executes the query asynchronously against Entity Framework. Adding async at this level would be misleading and unnecessary, as no I/O-bound operation occurs in this method.
 * We noticed we do not have a table for customers, it is not good to have just the customer name in the rental.
   Can you help us add a new entity for this? Don't forget to change the customer name field to a foreign key, and fix your previous method!
   * A new Customer entity was introduced to normalize the data model and avoid storing customer information as a plain string in the Rental table. The Rental entity was updated to reference Customer through a foreign key (CustomerId), establishing a proper many-to-one relationship. This ensures referential integrity, reduces data duplication, and makes the domain easier to evolve with additional customer attributes in the future. The DbContext was updated to include the new Customers table, allowing Entity Framework to manage the relationship and generate the corresponding database schema through migrations.
 * In the MovieFeatures class, there is a method to list all movies, tell us your opinion about it.
    * The original method eagerly loads all records with tracking enabled, which does not scale and limits flexibility. Returning an IQueryable with AsNoTracking improves performance, scalability, and allows higher layers to control query execution and filtering.
 * No exceptions are being caught in this api, how would you deal with these exceptions?
   * A global exception-handling middleware was introduced. This middleware intercepts all unhandled exceptions thrown during the request pipeline, logs them, and converts them into standardized HTTP responses. By registering the middleware using app.UseMiddleware<ExceptionHandlingMiddleware>(), the application ensures that exceptions are handled in a single, centralized place, instead of spreading try/catch blocks across controllers and services. This approach improves maintainability, enforces consistent error responses, and keeps the application layers clean and focused on their responsibilities. 


	## Challenge (Nice to have)
We need to implement a new feature in the system that supports automatic payment processing. Given the advancements in technology, it is essential to integrate multiple payment providers into our system.

Here are the specific instructions for this implementation:

* Payment Provider Classes:
    * In the "PaymentProvider" folder, you will find two classes that contain basic (dummy) implementations of payment providers. These can be used as a starting point for your work.
* RentalFeatures Class:
    * Within the RentalFeatures class, you are required to implement the payment processing functionality.
* Payment Provider Designation:
    * The specific payment provider to be used in a rental is specified in the Rental model under the attribute named "PaymentMethod".
* Extensibility:
    * The system should be designed to allow the addition of more payment providers in the future, ensuring flexibility and scalability.
* Payment Failure Handling:
    * If the payment method fails during the transaction, the system should prevent the creation of the rental record. In such cases, no rental should be saved to the database.

## Notes for the Challenge (Nice to Have)
The rental flow was refactored to improve separation of concerns, readability, and maintainability. A Payment Provider Factory was introduced to encapsulate the selection of payment strategies based on the payment method, decoupling the rental logic from concrete implementations and aligning with the Open/Closed Principle. The rental process was split into well-defined private methods responsible for validation, price calculation, payment processing, and persistence, ensuring that a rental is only saved after a successful payment.

This approach improves testability and enforces clear business rules. Future improvements may include payment rollback or compensation mechanisms, transactional workflows for multi-step operations, and more robust error handling (e.g., retry policies or an outbox pattern) to handle partial failures and external payment inconsistencies.