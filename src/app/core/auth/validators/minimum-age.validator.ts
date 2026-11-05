import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function minimumAgeValidator(minimumAge: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null;
    }

    const birthDate = new Date(`${control.value}T00:00:00`);
    const today = new Date();

    if (Number.isNaN(birthDate.getTime()) || birthDate > today) {
      return {
        minimumAge: {
          requiredAge: minimumAge,
          actualAge: null,
        },
      };
    }

    let age = today.getFullYear() - birthDate.getFullYear();

    const hasNotHadBirthday =
      today.getMonth() < birthDate.getMonth() ||
      (today.getMonth() === birthDate.getMonth() && today.getDate() < birthDate.getDate());

    if (hasNotHadBirthday) {
      age--;
    }

    return age >= minimumAge
      ? null
      : {
          minimumAge: {
            requiredAge: minimumAge,
            actualAge: age,
          },
        };
  };
}
