#ifndef _HR_HELLOWORLDCOMMAND_H_
#define _HR_HELLOWORLDCOMMAND_H_

#include "HrFramework/HrFrame.h"

namespace Hr
{
	class HrHelloWorldCommand : public HrCommand
	{
	public:
		virtual void Trigger() override;
		virtual void Execute() override;
	};
}

#endif




