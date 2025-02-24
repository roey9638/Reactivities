import { MenuItem } from "@mui/material";
import { ReactNode } from "react";
import { NavLink } from "react-router";

export default function MenuItemLink({ children, to }: { children: ReactNode, to: string }) {
    return (
        <MenuItem
            component={NavLink}
            to={to}
            sx={{
                fontSize: '1.2rem',
                textTransform: 'uppercase',
                fontWeight: 'bold',
                color: 'inherit',

                //  This is to [target] the [Active] [Property/ Component]
                // And if a [Certain] [Component] is [Active]. Will just [provide] a [style]
                '&.active' : {
                    color: 'yellow'
                }
            }}
        >
            {children}
        </MenuItem>
    )
}

/*
    What The [children] from [Above] means is [What] is in [Between] the [<MenuItem> </MenuItem>].
    // For Example [Bellow] VVV 
    // The [children] are [<Group fontSize="large" />] AND [<Typography> </Typography>]

    <MenuItem component={NavLink} to='/' sx={{ display: 'flex', gap: 2 }}>
        <Group fontSize="large" />
        <Typography variant="h4" fontWeight='bold'>Reactivities</Typography>
    </MenuItem>
*/