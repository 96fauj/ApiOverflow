using System;
using System.Collections.Generic;

namespace CsvApp.Business
{
    //Validation:
    //• You should not be able to load the same entry twice
    // - Maintain a collection .. choose the right collection which doesn't allow adding the same key twice.. hashset..HashSet<T>
    //• A meter reading must be associated with an Account
    // todo ensure we are not making multiple calls to the database, or figure out the most cost effective way to validate an existing customer in the database.

    // ID to be deemed valid
    //• Reading values should be in the format NNNNN
    // todo configurable regex maybe?

    public class Validation<T>
    {

        public bool IsRowValid(string row)
        {


            throw new NotImplementedException();
        }

        public IEnumerable<T> ValidatedRows()
        {
            throw new NotImplementedException();
        }

        public bool IsValidMeterReadValue(string value)
        {
            // should not be a negative value

            // should be in NNNNN format

            // should not be null

            // todo maybe stick in to settings and a regex pattern to validate against?

            throw new NotImplementedException();
        }


    }

    public interface IValidate
    {

    }
}