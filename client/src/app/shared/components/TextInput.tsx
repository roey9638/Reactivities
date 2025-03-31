import { TextField, TextFieldProps } from "@mui/material";
import { FieldValues, useController, UseControllerProps } from "react-hook-form"

// The [& UseControllerProps & TextFieldProps] will [allow] us to use alll there [Props].
// The [<T>] is Because we use [ActivitySchema] that has some [Props] VVV
// But we also want to use [FieldValues] And that's why we use [extends FieldValues].
type Props<T extends FieldValues> = {} & UseControllerProps<T> & TextFieldProps

export default function TextInput<T extends FieldValues>(props: Props<T>) {

    const { field, fieldState } = useController({ ...props });

    return (
        <TextField
            {...props} // Pass all props (e.g., placeholder, label, etc.)
            {...field} // Connect field properties (value, onChange, etc.)
            value={field.value || ''} // Ensures it doesn't break if `undefined`
            fullWidth
            variant="outlined"
            error={!!fieldState.error} // That's for making the [Input Border] [Red] if we didn't put anything.
            helperText={fieldState.error?.message} // That's for showing the [Error] about the [requirement] for the [fields].
        />
    )
}