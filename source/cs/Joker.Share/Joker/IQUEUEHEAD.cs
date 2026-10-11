using System.Runtime.CompilerServices;

namespace Joker;

internal struct IQUEUEHEAD
{
	public unsafe IQUEUEHEAD* next;

	public unsafe IQUEUEHEAD* prev;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void iqueue_init(IQUEUEHEAD* ptr)
	{
		ptr->next = ptr;
		ptr->prev = ptr;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static IKCPSEG* iqueue_entry(IQUEUEHEAD* ptr)
	{
		return (IKCPSEG*)ptr;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static bool iqueue_is_empty(IQUEUEHEAD* entry)
	{
		return entry == entry->next;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void iqueue_del(IQUEUEHEAD* entry)
	{
		entry->next->prev = entry->prev;
		entry->prev->next = entry->next;
		entry->next = null;
		entry->prev = null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void iqueue_del_init(IQUEUEHEAD* entry)
	{
		iqueue_del(entry);
		iqueue_init(entry);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void iqueue_add(IQUEUEHEAD* node, IQUEUEHEAD* head)
	{
		node->prev = head;
		node->next = head->next;
		head->next->prev = node;
		head->next = node;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void iqueue_add_tail(IQUEUEHEAD* node, IQUEUEHEAD* head)
	{
		node->prev = head->prev;
		node->next = head;
		head->prev->next = node;
		head->prev = node;
	}
}
