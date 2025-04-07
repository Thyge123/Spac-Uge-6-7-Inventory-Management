import { DataTable } from '@/components/ui/DataTable';
import type { ApiError } from '@/types';
import type { UseQueryResult } from '@tanstack/react-query';
import type { ColumnDef } from '@tanstack/react-table';
import { useParams } from 'react-router-dom';

type Props = {
    dataName: string,
    query: (id: string) => UseQueryResult<any, ApiError>;
    columns: ColumnDef<any>[];
};

export const DetailView: React.FC<Props> = ({ dataName, query, columns }) => {
    const { id } = useParams<{ id: string; }>();
    const { data, isLoading, error } = query(id!);

    if (isLoading) {
        return <div className="p-6">Loading {dataName}...</div>;
    }

    if (error) {
        return <div className="p-6 text-red-500">Error: {error.message}</div>;
    }

    return (
        <DataTable columns={columns} data={[data!]} />
    );
};