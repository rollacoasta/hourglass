﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInput.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// A representation of an input for a <see cref="Timer"/>.
    /// </summary>
    public abstract class TimerInput
    {
        /// <summary>
        /// The configuration data for the timer.
        /// </summary>
        private readonly TimerOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInput"/> class.
        /// </summary>
        /// <param name="options">The configuration data for the timer.</param>
        protected TimerInput(TimerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.options = TimerOptions.FromTimerOptions(options);
            this.options.Freeze();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInput"/> class from a <see cref="TimerInputInfo"/>.
        /// </summary>
        /// <param name="inputInfo">A <see cref="TimerInputInfo"/>.</param>
        protected TimerInput(TimerInputInfo inputInfo)
        {
            if (inputInfo == null)
            {
                throw new ArgumentNullException("inputInfo");
            }

            this.options = TimerOptions.FromTimerOptionsInfo(inputInfo.Options);
            this.options.Freeze();
        }

        /// <summary>
        /// Gets the configuration data for the timer.
        /// </summary>
        public TimerOptions Options
        {
            get { return this.options; }
        }

        /// <summary>
        /// Returns a <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input, favoring a <see cref="DateTimeTimerInput"/> in the case of ambiguity.
        /// </summary>
        /// <param name="str">An input <see cref="string"/>.</param>
        /// <returns>A <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input.</returns>
        public static TimerInput FromDateTimeOrTimeSpan(string str)
        {
            DateTime dateTime;
            if (DateTimeUtility.TryParseNatural(str, out dateTime))
            {
                return new DateTimeTimerInput(dateTime);
            }

            TimeSpan timeSpan;
            if (TimeSpanUtility.TryParseNatural(str, out timeSpan))
            {
                return new TimeSpanTimerInput(timeSpan);
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input, favoring a <see cref="TimeSpanTimerInput"/> in the case of ambiguity.
        /// </summary>
        /// <param name="str">An input <see cref="string"/>.</param>
        /// <returns>A <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input.</returns>
        public static TimerInput FromTimeSpanOrDateTime(string str)
        {
            TimeSpan timeSpan;
            if (TimeSpanUtility.TryParseNatural(str, out timeSpan))
            {
                return new TimeSpanTimerInput(timeSpan);
            }

            DateTime dateTime;
            if (DateTimeUtility.TryParseNatural(str, out dateTime))
            {
                return new DateTimeTimerInput(dateTime);
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="TimerInput"/> for the specified <see cref="TimerInputInfo"/>, or <c>null</c> if the
        /// <see cref="TimerInputInfo"/> is not a supported type.
        /// </summary>
        /// <param name="inputInfo">A <see cref="TimerInputInfo"/>.</param>
        /// <returns>A <see cref="TimerInput"/> for the specified <see cref="TimerInputInfo"/>, or <c>null</c> if the
        /// <see cref="TimerInputInfo"/> is not a supported type.</returns>
        public static TimerInput FromTimerInputInfo(TimerInputInfo inputInfo)
        {
            TimeSpanTimerInputInfo timeSpanTimerInputInfo = inputInfo as TimeSpanTimerInputInfo;
            if (timeSpanTimerInputInfo != null)
            {
                return new TimeSpanTimerInput(timeSpanTimerInputInfo);
            }

            DateTimeTimerInputInfo dateTimeTimerInputInfo = inputInfo as DateTimeTimerInputInfo;
            if (dateTimeTimerInputInfo != null)
            {
                return new DateTimeTimerInput(dateTimeTimerInputInfo);
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object, or <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            TimerInput input = (TimerInput)obj;
            return object.Equals(this.options, input.options);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = (31 * hashCode) + this.options.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerInput"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerInput"/> used for XML serialization.</returns>
        public TimerInputInfo ToTimerInputInfo()
        {
            TimerInputInfo info = this.GetNewTimerInputInfo();
            this.SetTimerInputInfo(info);
            return info;
        }

        /// <summary>
        /// Returns a new <see cref="TimerInputInfo"/> of the correct type for this class.
        /// </summary>
        /// <returns>A new <see cref="TimerInputInfo"/>.</returns>
        protected abstract TimerInputInfo GetNewTimerInputInfo();

        /// <summary>
        /// Sets the properties on a <see cref="TimerInputInfo"/> from the values in this class.
        /// </summary>
        /// <param name="timerInputInfo">A <see cref="TimerInputInfo"/>.</param>
        protected virtual void SetTimerInputInfo(TimerInputInfo timerInputInfo)
        {
            timerInputInfo.Options = TimerOptionsInfo.FromTimerOptions(this.options);
        }
    }
}