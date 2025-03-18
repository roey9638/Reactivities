import { Box, debounce, List, ListItemButton, TextField, Typography } from "@mui/material";
import axios from "axios";
import { useEffect, useMemo, useState } from "react";
import { FieldValues, useController, UseControllerProps } from "react-hook-form"

type Props<T extends FieldValues> = {
    label: string;
} & UseControllerProps<T>

export default function LocationInput<T extends FieldValues>(props: Props<T>) {

    const { field, fieldState } = useController({ ...props });
    const [loading, setLoading] = useState(false);
    const [suggestion, setSuggestions] = useState<LocationIQSuggestion[]>([]);
    const [inputValue, setInputValue] = useState(field.value || '');

    useEffect(() => {
        if (field.value && typeof field.value === 'object') {
            setInputValue(field.value.venue || '');
        } else {
            setInputValue(field.value || '');
        }
    }, [field.value])


    const locationUrl = 'https://api.locationiq.com/v1/autocomplete?key=pk.d82bbbc11d04dac80fe2cf003db9fe18&limit=5&dedupe=1&'

    const fetchSuggestion = useMemo(
        () => debounce(async (query: string) => {
            if (!query || query.length < 3) {
                setSuggestions([]);
                return;
            }

            setLoading(true);

            try {
                const res = await axios.get<LocationIQSuggestion[]>(`${locationUrl}q=${query}`);
                setSuggestions(res.data);
            } catch (error) {
                console.log(error);
            } finally {
                setLoading(false);
            }
        }, 500), [locationUrl]
    );

    const handleChange = async (value: string) => {
        field.onChange(value);
        await fetchSuggestion(value);
    }


    const handleSelect = (location: LocationIQSuggestion) => {
        const city = location.address?.city || location.address?.town || location.address?.village;
        const venue = location.display_name;
        const latitude = location.lat;
        const longitude = location.lon;

        setInputValue(venue);
        field.onChange({ city, venue, latitude, longitude });
        setSuggestions([]);
    }


    return (
        <Box>
            <TextField
                {...props}
                value={inputValue}
                onChange={e => handleChange(e.target.value)}
                fullWidth
                variant="outlined"
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
            />
            {loading && <Typography>Loading...</Typography>}
            {suggestion.length > 0 && (
                <List sx={{ border: 1 }}>
                    {suggestion.map(suggestion => (
                        <ListItemButton
                            divider
                            key={suggestion.place_id}
                            onClick={() => handleSelect(suggestion)}
                        >
                            {suggestion.display_name}
                        </ListItemButton>
                    ))}
                </List>
            )}
        </Box>
    )
}