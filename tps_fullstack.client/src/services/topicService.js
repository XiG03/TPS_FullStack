import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/admin/topic';

export const getTopics = async () => {
    const response = await fetch(`${API_BASE_URL}/getall`, {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch topics');
    const data = await response.json();
    return { data };
};

export const getTopicDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch topic detail');
    const data = await response.json();
    return { data };
};

export const createTopic = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to create topic');
    // Backend returns Created() with no body on success
    return { success: true };
};

export const updateTopic = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to update topic');
    const data = await response.json();
    return { data };
};

export const deleteTopic = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to delete topic');
    return { success: true };
};
