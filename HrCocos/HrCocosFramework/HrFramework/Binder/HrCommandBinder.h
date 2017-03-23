#ifndef _HR_COMMANDBINDER_H_
#define _HR_COMMANDBINDER_H_

#include "HrFramework/Binder/HrBinder.h"

namespace Hr
{
	class HrCommandBinder : public HrBinder
	{
	//template < typename T, typename U>
	//	void Bind()
	//	{
	//		std::type_index typeIndex(typeid(T));
	//		auto itemBindingInfo = m_mapBindingCommands.find(typeIndex);
	//		if (itemBindingInfo != m_mapBindingCommands.end())
	//		{
	//			HrCommandPtr pCommand = HrCheckPointerCast<HrCommand>(std::make_shared<U>());
	//			itemBindingInfo->second.push_back(pCommand);
	//		}
	//		else
	//		{
	//			std::list<HrCommandPtr> lisCommand;
	//			HrCommandPtr pCommand = HrCheckPointerCast<HrCommand>(std::make_shared<U>());
	//			lisCommand.push_back(pCommand);
	//			m_mapBindingCommands[typeIndex] = lisCommand;
	//		}
	//	}

	//protected:
	//	std::unordered_map<std::type_index, std::list<HrCommandPtr> > m_mapBindingCommands;
		
	};
}

#endif



