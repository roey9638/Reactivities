import { FormControl, FormHelperText, InputLabel, MenuItem, Select } from "@mui/material";
import { SelectInputProps } from "@mui/material/Select/SelectInput";
import { FieldValues, useController, UseControllerProps } from "react-hook-form"

// The [& UseControllerProps & SelectInputProps] will [allow] us to use alll there [Props].
// The [<T>] is Because we use [ActivitySchema] that has some [Props] VVV
// But we also want to use [FieldValues] And that's why we use [extends FieldValues].
type Props<T extends FieldValues> = {
    label: string;
    items: { text: string, value: string }[];
} & UseControllerProps<T> & Partial<SelectInputProps>

export default function SelectInput<T extends FieldValues>(props: Props<T>) {

    const { field, fieldState } = useController({ ...props });

    return (
        <FormControl fullWidth error={!!fieldState.error}>
            <InputLabel>{props.label}</InputLabel>
            <Select
                value={field.value || ''}
                label={props.label}
                onChange={field.onChange}
            >
                {props.items.map(item => (
                    <MenuItem key={item.value} value={item.value}>
                        {item.text}
                    </MenuItem>
                ))}
            </Select>
            <FormHelperText>{fieldState.error?.message}</FormHelperText>
        </FormControl>
    )
}